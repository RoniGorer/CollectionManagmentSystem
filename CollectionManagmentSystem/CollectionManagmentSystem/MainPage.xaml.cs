using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace CollectionManagmentSystem
{
    public partial class MainPage : ContentPage
    {
        private List<Collection> collections;
        private string dataFolderPath;

        public MainPage()
        {
            InitializeComponent();

            dataFolderPath = FileManager.GetDataFolderPath();
            Trace.WriteLine($"Data folder path initialized: {dataFolderPath}");

            collections = FileManager.LoadCollections(dataFolderPath);
            DisplayCollections();
        }

        private void DisplayCollections()
        {
            Trace.WriteLine("DisplayCollections method called");
            CollectionStackLayout.Children.Clear();

            foreach (var collection in collections)
            {
                
                var titleLabel = new Label
                {
                    Text = collection.Name,
                    FontSize = 20,  
                    Margin = new Thickness(0, 5)
                };

                var descriptionLabel = new Label
                {
                    Text = collection.Description,
                    FontSize = 16,  
                    Margin = new Thickness(0, 5)
                };

                var editButton = new Button { Text = "Edit" };
                editButton.Clicked += (sender, e) => EditCollection(collection);

                var deleteButton = new Button { Text = "Delete" };
                deleteButton.Clicked += (sender, e) => DeleteCollection(collection);

                var collectionLayout = new StackLayout { Orientation = StackOrientation.Vertical };
                collectionLayout.Children.Add(titleLabel);
                collectionLayout.Children.Add(descriptionLabel);
                collectionLayout.Children.Add(editButton);
                collectionLayout.Children.Add(deleteButton);

                CollectionStackLayout.Children.Add(collectionLayout);
            }
        }

        private async void AddNewCollectionButton_Clicked(object sender, EventArgs e)
        {
            Trace.WriteLine("AddNewCollectionButton_Clicked called");

            var collectionName = await DisplayPromptAsync("New Collection", "Enter collection name:");
            if (!string.IsNullOrWhiteSpace(collectionName))
            {
                var collectionDescription = await DisplayPromptAsync("New Collection", "Enter collection description:");
                if (!string.IsNullOrWhiteSpace(collectionDescription))
                {
                    var newCollection = new Collection
                    {
                        Name = collectionName,
                        Description = collectionDescription
                    };
                    collections.Add(newCollection);

                    FileManager.SaveCollections(collections, dataFolderPath);
                    DisplayCollections();
                }
            }
        }

        private async void EditCollection(Collection collection)
        {
            Trace.WriteLine("EditCollection method called");

            var newName = await DisplayPromptAsync("Edit Collection", "Enter new name:", initialValue: collection.Name);
            if (!string.IsNullOrWhiteSpace(newName))
            {
                var newDescription = await DisplayPromptAsync("Edit Collection", "Enter new description:", initialValue: collection.Description);
                if (!string.IsNullOrWhiteSpace(newDescription))
                {
                    collection.Name = newName;
                    collection.Description = newDescription;

                    FileManager.SaveCollections(collections, dataFolderPath);
                    DisplayCollections();
                }
            }
        }

        private void DeleteCollection(Collection collection)
        {
            Trace.WriteLine("DeleteCollection method called");

            collections.Remove(collection);
            var filePath = Path.Combine(dataFolderPath, $"{collection.Name}.txt");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            FileManager.SaveCollections(collections, dataFolderPath);
            DisplayCollections();
        }
    }
}
