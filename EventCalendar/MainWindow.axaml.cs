using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;

namespace EventCalendar;

public partial class MainWindow : Window
{
    
    private List<Event> _events = new();
    public MainWindow()
    {
        InitializeComponent();
    }
    
    private void EventAdd_OnClick(object? sender, RoutedEventArgs e)
    {
        var newEvent = new Event
        {
            Name = EventName.Text,
            Description = EventDesc.Text,
            Day = (EventDay.SelectedItem as ComboBoxItem)?.Content as string,
            IsImportant = EventImportant.IsChecked ?? false
        };

        if (newEvent.Name == null || newEvent.Description == null || newEvent.Day == null)
        {
            var messageBox = new Window
            {
                Title = "Błąd",
                Content = "Wypełnij wszystkie pola!",
                Width = 200,
                Height = 50,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            
            messageBox.Show();

            return;
        }
        
        _events.Add(newEvent);
        
        EventName.Clear();
        EventDesc.Clear();
        EventDay.SelectedItem = null;
        
        RefreshEventList();
    }

    public void RefreshEventList()
    {
        EventList.Items.Clear();

        string selectedDayFilter = (EventDayFilter.SelectedItem as ComboBoxItem)?.Content as string ?? "Wszystko";
        
        var filteredEvents = _events.Where(eventItem => 
            selectedDayFilter == "Wszystko" || eventItem.Day == selectedDayFilter).ToList();
        
        foreach(var eventItem in filteredEvents)
        {
            var eventPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(10)
            };

            var nameBlock = new TextBlock
            {
                Text = eventItem.Name,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10)
            };

            var dayBlock = new TextBlock
            {
                Text = eventItem.Day,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10)
            };

            var importantBlock = new TextBlock
            {
                Text = eventItem.IsImportant ? "Ważne wydarzenie" : "Zwykłe wydarzenie",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10)
            };

            var importantCheckBox = new CheckBox
            {
                IsChecked = eventItem.IsImportant,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10)
            };
            
            var detailsButton = new Button
            {
                Content = "Pokaż szczegóły",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10)
            };

            importantCheckBox.Checked += (sender, args) => ToggleImportant(eventItem);
            importantCheckBox.Unchecked += (sender, args) => ToggleImportant(eventItem);
            
            detailsButton.Click += (_, _) => ShowEventDetails(eventItem);
            
            eventPanel.Children.Add(nameBlock);
            eventPanel.Children.Add(dayBlock);
            eventPanel.Children.Add(importantBlock);
            eventPanel.Children.Add(importantCheckBox);
            eventPanel.Children.Add(detailsButton);
            
            EventList.Items.Add(eventPanel);
        }
    }
    
    private void ToggleImportant(Event eventItem)
    {
        eventItem.IsImportant = !eventItem.IsImportant;
        RefreshEventList();
    }
    
    private void EventDayFilter_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        RefreshEventList();
    }

    private void ShowEventDetails(Event eventItem)
    {
        var detailsWindow = new Window
        {
            Title = $"Szczegóły wydarzenia {eventItem.Name}",
            Height = 100,
            Width = 700
        };

        var detailsItem = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Margin = new Thickness(10)
        };
        
        var nameBlock = new TextBlock
        {
            Text = eventItem.Name,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(10)
        };

        var descBlock = new TextBlock
        {
            Text = eventItem.Description,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(10)
        };

        var dayBlock = new TextBlock
        {
            Text = eventItem.Day,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(10)
        };

        var importantBlock = new TextBlock
        {
            Text = eventItem.IsImportant ? "Ważne wydarzenie" : "Zwykłe wydarzenie",
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(10)
        };

        var importantCheckBox = new CheckBox
        {
            IsChecked = eventItem.IsImportant,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(10)
        };

        importantCheckBox.Checked += (_, _) => 
        {
            ToggleImportant(eventItem);
            importantBlock.Text = "Ważne wydarzenie";
        };

        importantCheckBox.Unchecked += (_, _) => 
        {
            ToggleImportant(eventItem);
            importantBlock.Text = "Zwykłe wydarzenie";
        };
        
        detailsItem.Children.Add(nameBlock);
        detailsItem.Children.Add(descBlock);
        detailsItem.Children.Add(dayBlock);
        detailsItem.Children.Add(importantBlock);
        detailsItem.Children.Add(importantCheckBox);

        detailsWindow.Content = detailsItem;
        
        detailsWindow.Show();
    }
    
    public class Event
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Day { get; set; }
        public bool IsImportant { get; set; }
    }
}