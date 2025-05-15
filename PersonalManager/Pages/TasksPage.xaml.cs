using Microsoft.EntityFrameworkCore;
using PersonalManager.Data;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PersonalManager.Models;
using System.Collections.ObjectModel;
using Microsoft.VisualBasic;
using PersonalManager.Converters;
using System.Windows.Media;

namespace PersonalManager.Pages
{
    public partial class TasksPage : Page
    {
        private readonly AppDbContext Context;
        private int _currentTaskId = -1;
        private ObservableCollection<Task> _tasks = new ObservableCollection<Task>();

        public TasksPage(AppDbContext context)
        {
            InitializeComponent();
            Context = context;
            TasksList.ItemsSource = _tasks;
            LoadTodoLists();
            LoadTasks();
        }

        private void LoadTodoLists()
        {
            TodoListsComboBox.ItemsSource = Context.TodoLists
        .OrderBy(l => l.Title)
        .ToList();

            if (TodoListsComboBox.Items.Count > 0)
            {
                TodoListsComboBox.SelectedIndex = 0;
            }
        }

        private void TaskCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox)?.DataContext is Task task)
            {
                task.Status = "todo";
                Context.SaveChanges();
                LoadTasks();
            }
        }

        private void TaskCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox)?.DataContext is Task task)
            {
                task.Status = "done";
                Context.SaveChanges();
                LoadTasks();
            }
        }

        private void SaveTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dueDate = TaskDueDate.SelectedDate.Value.ToUniversalTime();

                if (string.IsNullOrWhiteSpace(TaskTitle.Text))
                {
                    MessageBox.Show("Введите название задачи");
                    return;
                }

                if (TaskDueDate.SelectedDate == null)
                {
                    MessageBox.Show("Укажите срок выполнения");
                    return;
                }

                if (TodoListsComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Выберите список задач");
                    return;
                }


                var selectedList = Context.TodoLists
                    .FirstOrDefault(l => l.ListId == ((TodoList)TodoListsComboBox.SelectedItem).ListId);

                if (selectedList == null)
                {
                    MessageBox.Show("Выбранный список не найден в базе данных");
                    return;
                }

                if (_currentTaskId == -1)
                {
                    var newTask = new Task
                    {
                        Title = TaskTitle.Text,
                        Description = TaskDescription.Text,
                        DueDate = dueDate,
                        CreatedAt = DateTime.UtcNow,
                        Status = "todo",
                        ListId = selectedList.ListId,
                        Priority = (TaskPriority.SelectedItem as ComboBoxItem)?.Tag?.ToString() // Добавляем приоритет
                    };
                    Context.Tasks.Add(newTask);
                }
                else
                {
                    var existingTask = Context.Tasks.FirstOrDefault(t => t.TaskId == _currentTaskId);
                    if (existingTask != null)
                    {
                        existingTask.Title = TaskTitle.Text;
                        existingTask.Description = TaskDescription.Text;
                        existingTask.DueDate = dueDate;
                        existingTask.UpdatedAt = DateTime.UtcNow; // Используем UTC и здесь
                        existingTask.ListId = selectedList.ListId;
                        existingTask.Priority = (TaskPriority.SelectedItem as ComboBoxItem)?.Tag?.ToString(); // Обновляем приоритет
                    }
                }

                // Дополнительная проверка перед сохранением
                var changes = Context.ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                    .ToList();


                Context.SaveChanges();
                NewTask_Click(sender, e);
                LoadTasks();
                MessageBox.Show("Задача сохранена");
            }
            catch (DbUpdateException dbEx)
            {
                string errorMessage = $"Ошибка базы данных: {dbEx.Message}";
                if (dbEx.InnerException != null)
                {
                    errorMessage += $"\nInner: {dbEx.InnerException.Message}";
                }
                MessageBox.Show(errorMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}\n\nStackTrace: {ex.StackTrace}");
            }
        }

        private void NewTask_Click(object sender, RoutedEventArgs e)
        {
            _currentTaskId = -1;
            TasksList.SelectedItem = null;
            TaskTitle.Text = "";
            TaskDescription.Text = "";
            TaskDueDate.SelectedDate = DateTime.Now;
            SubTasksList.Items.Clear();
            SubTasksList.ItemsSource = null;
            NewSubTaskTextBox.Text = "";

            if (TodoListsComboBox.Items.Count > 0)
            {
                TodoListsComboBox.SelectedIndex = 0;
            }
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_currentTaskId == -1)
                {
                    MessageBox.Show("Выберите задачу для удаления");
                    return;
                }

                // Вариант 1: Простое удаление
                var taskToDelete = Context.Tasks.Find(_currentTaskId);
                if (taskToDelete != null)
                {
                    Context.Tasks.Remove(taskToDelete);
                    Context.SaveChanges();
                    LoadTasks();
                    NewTask_Click(sender, e); // Сброс формы
                    MessageBox.Show("Задача успешно удалена");
                }
                else
                {
                    MessageBox.Show("Задача не найдена");
                }
            }
            catch (DbUpdateException dbEx)
            {
                MessageBox.Show($"Ошибка базы данных: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void LoadTasks()
        {
            try
            {
                _tasks.Clear();

                var selectedTask = TasksList.SelectedItem as Task;
                var selectedTaskId = selectedTask?.TaskId ?? -1;

                var tasks = Context.Tasks
                    .Include(t => t.TodoList)
                    .OrderBy(t => t.DueDate)
                    .ToList();

                foreach (var task in tasks)
                {
                    task.IsCompleted = task.Status == "done";
                    // Обновляем состояние просроченности
                    task.IsOverdue = task.Status != "done" && task.DueDate < DateTime.UtcNow;
                    _tasks.Add(task);
                }

                TasksList.ItemsSource = _tasks;

                if (selectedTaskId != -1)
                {
                    TasksList.SelectedItem = _tasks.FirstOrDefault(t => t.TaskId == selectedTaskId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке задач: {ex.Message}");
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TasksList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TasksList.SelectedItem is Task selectedTask)
            {
                _currentTaskId = selectedTask.TaskId;
                LoadSubTasks(_currentTaskId);

                // Обновляем поля формы
                TaskTitle.Text = selectedTask.Title;
                TaskDescription.Text = selectedTask.Description;
                TaskDueDate.SelectedDate = selectedTask.DueDate?.ToLocalTime();

                // Обновляем выбранный список
                if (selectedTask.TodoList != null)
                {
                    TodoListsComboBox.SelectedItem = selectedTask.TodoList;
                }
            }
            else
            {
                _currentTaskId = -1;
                SubTasksList.ItemsSource = null;
            }
        }

        private void LoadSubTasks(int taskId)
        {
            SubTasksList.ItemsSource = Context.TaskSubtasks
                .Where(st => st.TaskId == taskId)
                .OrderBy(st => st.IsCompleted)
                .ThenBy(st => st.CreatedAt)
                .ToList();
        }

        private void AddSubTask_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NewSubTaskTextBox.Text)) return;
            if (_currentTaskId == -1) return;

            var newSubTask = new TaskSubtask
            {
                Title = NewSubTaskTextBox.Text.Trim(),
                IsCompleted = false,
                TaskId = _currentTaskId,
                CreatedAt = DateTime.UtcNow
            };

            Context.TaskSubtasks.Add(newSubTask);
            Context.SaveChanges();

            NewSubTaskTextBox.Text = "";
            LoadSubTasks(_currentTaskId);
        }

        private void SubTaskCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox)?.DataContext is TaskSubtask subTask)
            {
                subTask.IsCompleted = true;
                subTask.CompletedAt = DateTime.UtcNow;
                Context.SaveChanges();
                LoadSubTasks(_currentTaskId);
            }
        }

        private void SubTaskCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox)?.DataContext is TaskSubtask subTask)
            {
                subTask.IsCompleted = false;
                subTask.CompletedAt = null;
                Context.SaveChanges();
                LoadSubTasks(_currentTaskId);
            }
        }

        private void DeleteSubTask_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button { Tag: int subtaskId })
            {
                var subTask = Context.TaskSubtasks.Find(subtaskId);
                if (subTask != null)
                {
                    Context.TaskSubtasks.Remove(subTask);
                    Context.SaveChanges();
                    LoadSubTasks(_currentTaskId);
                }
            }
        }

        private void AddNewList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string newListName = Microsoft.VisualBasic.Interaction.InputBox(
                    "Введите название нового списка задач:",
                    "Новый список задач",
                    "Новый список");

                if (!string.IsNullOrWhiteSpace(newListName))
                {
                    // Проверка на существование списка с таким именем
                    if (Context.TodoLists.Any(l => l.Title == newListName.Trim()))
                    {
                        MessageBox.Show("Список с таким названием уже существует");
                        return;
                    }

                    var newList = new TodoList
                    {
                        Title = newListName.Trim(),
                        UserId = 1, // Здесь нужно использовать ID текущего пользователя
                        CreatedAt = DateTime.UtcNow,
                        IsArchived = false
                    };

                    Context.TodoLists.Add(newList);
                    Context.SaveChanges();

                    LoadTodoLists();
                    TodoListsComboBox.SelectedItem = newList;

                    MessageBox.Show("Новый список задач создан");
                }
            }
            catch (DbUpdateException dbEx)
            {
                MessageBox.Show($"Ошибка базы данных: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании списка: {ex.Message}");
            }
        }
    }
}