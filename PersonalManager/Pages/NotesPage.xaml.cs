using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using PersonalManager.Data;
using PersonalManager.Models;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace PersonalManager.Pages
{
    public partial class NotesPage : Page
    {
        private readonly AppDbContext _context;
        private int _currentNoteId = -1;
        private bool _isLoading = false;

        public NotesPage(AppDbContext context)
        {
            InitializeComponent();
            _context = context;
            PinButton.IsChecked = false;
            ArchiveButton.IsChecked = false;
            LoadNotes();
        }

        private void LoadNotes(string searchText = null, bool? showPinned = null, bool? showArchived = null)
        {
            try
            {
                var query = _context.Notes
                    .Include(n => n.NoteAttachments) // Загружаем вложения
                    .AsQueryable();

                // Фильтрация по поисковому запросу
                if (!string.IsNullOrEmpty(searchText) && searchText != "Поиск заметок...")
                {
                    query = query.Where(n => n.Title.Contains(searchText) ||
                                     n.Content.Contains(searchText));
                }

                // Фильтрация по статусу закрепления
                if (showPinned.HasValue)
                {
                    query = query.Where(n => n.IsPinned == showPinned);
                }

                // Фильтрация по статусу архивации
                if (showArchived.HasValue)
                {
                    query = query.Where(n => n.IsArchived == showArchived);
                }
                else
                {
                    // По умолчанию не показываем архивные
                    query = query.Where(n => n.IsArchived != true);
                }

                NotesList.ItemsSource = query
                    .OrderByDescending(n => n.IsPinned)
                    .ThenByDescending(n => n.UpdatedAt ?? n.CreatedAt)
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке заметок: {ex.Message}");
            }
        }

        private void NotesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isLoading) return;

            if (NotesList.SelectedItem is Note selectedNote)
            {
                _isLoading = true;

                _currentNoteId = selectedNote.NoteId;
                NoteTitle.Text = selectedNote.Title;
                NoteContent.Document.Blocks.Clear();
                NoteContent.Document.Blocks.Add(new Paragraph(new Run(selectedNote.Content)));

                PinButton.IsChecked = selectedNote.IsPinned ?? false;
                ArchiveButton.IsChecked = selectedNote.IsArchived ?? false;

                // Загружаем вложения для выбранной заметки
                LoadAttachments(selectedNote.NoteId);

                _isLoading = false;
            }
        }

        private void LoadAttachments(int noteId)
        {
            try
            {
                var attachments = _context.NoteAttachments
                    .Where(a => a.NoteId == noteId)
                    .ToList();
                AttachmentsList.ItemsSource = attachments;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке вложений: {ex.Message}");
            }
        }

        private void NewNote_Click(object sender, RoutedEventArgs e)
        {
            _currentNoteId = -1;
            NoteTitle.Text = "";
            NoteContent.Document.Blocks.Clear();
            PinButton.IsChecked = false;
            ArchiveButton.IsChecked = false;
            NotesList.SelectedItem = null;
            AttachmentsList.ItemsSource = null;
        }

        private void DeleteNote_Click(object sender, RoutedEventArgs e)
        {
            if (_currentNoteId != -1)
            {
                var result = MessageBox.Show("Вы уверены, что хотите удалить эту заметку вместе со всеми вложениями?",
                    "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    var note = _context.Notes
                        .Include(n => n.NoteAttachments)
                        .FirstOrDefault(n => n.NoteId == _currentNoteId);
                    if (note != null)
                    {
                        // Удаляем все вложения заметки
                        _context.NoteAttachments.RemoveRange(note.NoteAttachments);
                        _context.Notes.Remove(note);
                        _context.SaveChanges();
                        LoadNotes();
                        NewNote_Click(sender, e); // Сброс формы
                    }
                }
            }
        }

        private void SaveNote_Click(object sender, RoutedEventArgs e)
        {
            string title = NoteTitle.Text.Trim();
            string content = new TextRange(
                NoteContent.Document.ContentStart,
                NoteContent.Document.ContentEnd
            ).Text.Trim();

            if (string.IsNullOrWhiteSpace(title) && string.IsNullOrWhiteSpace(content))
            {
                MessageBox.Show("Заметка не может быть пустой", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_currentNoteId == -1)
            {
                // Новая заметка
                var newNote = new Note
                {
                    Title = title,
                    Content = content,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IsPinned = PinButton.IsChecked ?? false,
                    IsArchived = ArchiveButton.IsChecked ?? false,
                    UserId = 1 // Здесь должен быть ID текущего пользователя
                };

                _context.Notes.Add(newNote);
                _context.SaveChanges();
                _currentNoteId = newNote.NoteId; // Устанавливаем ID новой заметки
            }
            else
            {
                // Обновление существующей
                var note = _context.Notes.Find(_currentNoteId);
                if (note != null)
                {
                    note.Title = title;
                    note.Content = content;
                    note.UpdatedAt = DateTime.Now;
                    note.IsPinned = PinButton.IsChecked ?? false;
                    note.IsArchived = ArchiveButton.IsChecked ?? false;
                    _context.Notes.Update(note);
                    _context.SaveChanges();
                }
            }

            LoadNotes();
        }

        private void AddAttachment_Click(object sender, RoutedEventArgs e)
        {
            if (_currentNoteId == -1)
            {
                MessageBox.Show("Сначала создайте или выберите заметку", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Title = "Выберите файлы для прикрепления"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    // Создаем папку Attachments, если ее нет
                    string attachmentsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Attachments");
                    if (!Directory.Exists(attachmentsFolder))
                    {
                        Directory.CreateDirectory(attachmentsFolder);
                    }

                    foreach (string filePath in openFileDialog.FileNames)
                    {
                        var fileInfo = new FileInfo(filePath);

                        // Генерируем уникальное имя файла
                        string uniqueFileName = Guid.NewGuid().ToString() + fileInfo.Extension;
                        string destinationPath = Path.Combine(attachmentsFolder, uniqueFileName);

                        // Копируем файл в папку Attachments
                        File.Copy(filePath, destinationPath);

                        var attachment = new NoteAttachment
                        {
                            NoteId = _currentNoteId,
                            FileUrl = destinationPath,
                            FileName = fileInfo.Name,
                            FileType = fileInfo.Extension,
                            FileSize = (int)fileInfo.Length,
                            CreatedAt = DateTime.Now
                        };

                        _context.NoteAttachments.Add(attachment);
                    }

                    _context.SaveChanges();
                    LoadAttachments(_currentNoteId);
                    LoadNotes();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении вложения: {ex.Message}");
                }
            }
        }

        private void DeleteAttachment_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is int attachmentId)
            {
                var result = MessageBox.Show("Удалить это вложение?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        var attachment = _context.NoteAttachments.Find(attachmentId);
                        if (attachment != null)
                        {
                            // Удаляем файл с диска
                            if (File.Exists(attachment.FileUrl))
                            {
                                File.Delete(attachment.FileUrl);
                            }

                            // Удаляем запись из базы данных
                            _context.NoteAttachments.Remove(attachment);
                            _context.SaveChanges();
                            LoadAttachments(_currentNoteId);
                            LoadNotes();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении вложения: {ex.Message}");
                    }
                }
            }
        }

        // Остальные методы остаются без изменений
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchBox == null || PinnedFilter == null || ArchivedFilter == null) return;

            LoadNotes(SearchBox.Text,
                     PinnedFilter.IsChecked,
                     ArchivedFilter.IsChecked);
        }

        private void PinButton_Checked(object sender, RoutedEventArgs e)
        {
            if (_isLoading || _currentNoteId == -1) return;

            try
            {
                var note = _context.Notes.Find(_currentNoteId);
                if (note != null)
                {
                    note.IsPinned = PinButton.IsChecked ?? false;
                    note.UpdatedAt = DateTime.Now;
                    _context.SaveChanges();

                    // Обновляем список с учетом текущих фильтров
                    LoadNotes(SearchBox.Text,
                             PinnedFilter.IsChecked ?? false ? true : (bool?)null,
                             ArchivedFilter.IsChecked ?? false ? true : (bool?)null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при изменении статуса закрепления: {ex.Message}");
            }
        }

        private void FilterToggle_Checked(object sender, RoutedEventArgs e)
        {
            if (SearchBox == null || PinnedFilter == null || ArchivedFilter == null) return;

            bool? showPinned = PinnedFilter.IsChecked ?? false ? true : null;
            bool? showArchived = ArchivedFilter.IsChecked ?? false ? true : null;

            LoadNotes(SearchBox.Text, showPinned, showArchived);
        }

        private void ArchiveButton_Checked(object sender, RoutedEventArgs e)
        {
            if (_isLoading || _currentNoteId == -1) return;

            try
            {
                var note = _context.Notes.Find(_currentNoteId);
                if (note != null)
                {
                    note.IsArchived = ArchiveButton.IsChecked ?? false;
                    note.UpdatedAt = DateTime.Now;
                    _context.SaveChanges();
                    LoadNotes(SearchBox.Text, PinnedFilter.IsChecked, ArchivedFilter.IsChecked);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при изменении статуса архивации: {ex.Message}");
            }
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchBox.Text == "Поиск заметок...")
            {
                SearchBox.Text = "";
                SearchBox.Foreground = Brushes.Black;
            }
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                SearchBox.Text = "Поиск заметок...";
                SearchBox.Foreground = Brushes.Gray;
            }
        }

        private void NoteTitle_GotFocus(object sender, RoutedEventArgs e)
        {
            if (NoteTitle.Text == "Заголовок заметки")
            {
                NoteTitle.Text = "";
                NoteTitle.Foreground = Brushes.Black;
            }
        }

        private void NoteTitle_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NoteTitle.Text))
            {
                NoteTitle.Text = "Заголовок заметки";
                NoteTitle.Foreground = Brushes.Gray;
            }
        }

        private void OpenAttachment_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is int attachmentId)
            {
                try
                {
                    var attachment = _context.NoteAttachments.Find(attachmentId);
                    if (attachment != null)
                    {
                        // Проверяем, существует ли файл
                        if (File.Exists(attachment.FileUrl))
                        {
                            // Открываем файл с помощью стандартного приложения
                            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                            {
                                FileName = attachment.FileUrl,
                                UseShellExecute = true
                            });
                        }
                        else
                        {
                            MessageBox.Show("Файл не найден по указанному пути.", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при открытии файла: {ex.Message}");
                }
            }
        }

        private void Attachment_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item && item.Content is NoteAttachment attachment)
            {
                try
                {
                    if (File.Exists(attachment.FileUrl))
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = attachment.FileUrl,
                            UseShellExecute = true
                        });
                    }
                    else
                    {
                        MessageBox.Show("Файл не найден.", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при открытии файла: {ex.Message}");
                }
            }
        }

        private void ReloadNotes_Click(object sender, RoutedEventArgs e)
        {
            LoadNotes();
        }
    }
}