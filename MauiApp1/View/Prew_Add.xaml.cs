using MauiApp1.Entities;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;

namespace MauiApp1.View;

public partial class Prew_Add : ContentPage
{

    DayTask task;
    List<int> FontSizes = new();
    public Prew_Add(DayTask task)
	{
		InitializeComponent();
        
        this.task = task;
        Type structType = typeof(System.Drawing.Color);
        PropertyInfo[] fields = structType.GetProperties();
        foreach (PropertyInfo field in fields)
        {
            TextColorPicker.Items.Add(field.Name);
            TitleColorPicker.Items.Add(field.Name);
        }
        for (int a = 0; a <= 8; a++)
        {
            TextColorPicker.Items.RemoveAt(TextColorPicker.Items.Count - 1);
            TitleColorPicker.Items.RemoveAt(TitleColorPicker.Items.Count - 1);
        }
        for (int i = 10; i < 60; i += 2)
        {
            FontSizes.Add(i);
        }
        FontsPicker.ItemsSource = FontSizes;
        FontsPicker.SelectedItem = (int)taskEditor.FontSize;
        TitleColorPicker.SelectedItem = "White";
        TextColorPicker.SelectedItem = "White";

        if (task != null)
            WindowPrepare();
    }


    private async void WindowPrepare()
    {
        try
        {
            TaskTitle.Text = task.Task;
            taskEditor.Text = task.TaskText;
            taskEditor.FontSize = task.TaskDecorations.EditorFontSize;
            taskEditor.TextColor = task.TaskDecorations.TaskDescriptionColorAsColor;
            TaskTitle.TextColor = task.TaskDecorations.TaskTitleColorAsColor;
            

            TitleColorPicker.SelectedItem = task.TaskDecorations.TaskTitleColor;
            TextColorPicker.SelectedItem = task.TaskDecorations.TaskDescriptionColor;
        }
        catch (Exception ex) { await DisplayAlert("Error", ex.Message, "Ok"); }
    }

    private void ChangeEditorFontSize(object sender, EventArgs e)
    {
        taskEditor.FontSize = (int)FontsPicker.SelectedItem;
    }

    private async void ChangeTitleTextColor(object sender, EventArgs e)
    {
        try
        {
            TaskTitle.TextColor = Microsoft.Maui.Graphics.Color.Parse(TitleColorPicker.SelectedItem as string);
        }
        catch (Exception ex) { await DisplayAlert("Error", ex.Message, "Ok"); }
    }

    private async void ChangeEditorTextColor(object sender, EventArgs e)
    {
        try
        {
            taskEditor.TextColor = Microsoft.Maui.Graphics.Color.Parse(TextColorPicker.SelectedItem as string);
        }
        catch (Exception ex) { await DisplayAlert("Error", ex.Message, "Ok"); }
    }
    private async void AddTask(object sender, EventArgs e)
    {
        try
        {
            if(task == null)
            {
                if (string.IsNullOrEmpty(TaskTitle.Text))
                {
                    await DisplayAlert("Error", "Задача должна состоять хотя бы из одного не пробельного символа", "Ok");
                }
                else
                {
                    using (AppContext db = new AppContext())
                    {
                        DayTask task = new DayTask() { DayToComplete = TaskDate.Date, Task = TaskTitle.Text, IsCompleted = false, TaskText = taskEditor.Text };
                        TaskDecorations decorations = new TaskDecorations { EditorFontSize = (int)FontsPicker.SelectedItem, TaskDescriptionColor = (string)TextColorPicker.SelectedItem != null ? (string)TextColorPicker.SelectedItem : "White", TaskTitleColor = (string)TitleColorPicker.SelectedItem != null ? (string)TitleColorPicker.SelectedItem : "White" };
                        if (task.Task.Trim().Length == 0)
                        {
                            await DisplayAlert("Error", "Укажите задачу", "Ok");
                        }
                        else
                        {
                            var added = db.TaskDecorations.Add(decorations);
                            task.TaskDecorations = added.Entity;
                            db.Tasks.Add(task);
                            db.SaveChanges();
                            await DisplayAlert("Успех", "Задача добавлена", "ок");
                            await Navigation.PopAsync();
                        }
                    }
                }
                
            }
            else
            {
                task.TaskDecorations.EditorFontSize = (int)FontsPicker.SelectedItem;
                task.TaskDecorations.TaskDescriptionColor = (string)TextColorPicker.SelectedItem;
                task.TaskDecorations.TaskTitleColor = (string)TitleColorPicker.SelectedItem;
                task.TaskText = (string)taskEditor.Text;
                task.Task = TaskTitle.Text;
                using (AppContext db = new AppContext())
                {
                    db.TaskDecorations.Update(task.TaskDecorations);
                    db.Tasks.Update(task);
                    db.SaveChanges();
                }
            }
            
            
        }
        catch (Exception ex) { await DisplayAlert("Error", ex.Message, "Ok"); }
    }

    private async void ContentPage_NavigatedFrom(object sender, NavigatedFromEventArgs e)
    {
        await Navigation.PopAsync();
        PrewMainPage.isOpen = false;
    }
}