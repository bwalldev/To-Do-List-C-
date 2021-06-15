/* 
    Name: To-do list application
    Desc: Keeps track of things that the user needs to get done 
    Date: 6/9/21
*/

using System;
using System.IO;
using System.Collections.Generic;

namespace To_Do_List
{
    ////////////////////////////////////////////////////////////////
    /// 
    /// TODOLIST CLASS
    /// 
    ////////////////////////////////////////////////////////////////
    
    class ToDoList
    {
        // UPDATE SAFE FILE LOCATION
        string save_data_path = @"C:\Users\Brandon\source\repos\To-Do List\To-Do List\SavedData.txt";

        List<Task> task_list = new List<Task>();

        public ToDoList()
        {
            if (File.Exists(save_data_path))
                loadData();
        }

        public void pause()
        {
            Console.Write("\n*** Press [ENTER] to continue ***");
            Console.ReadLine();
        }

        public void loadData()
        {
            string[] data = File.ReadAllLines(save_data_path);
            Task tempTask;
            bool isComplete;

            foreach(string line in data)
            {
                string[] values = line.Split(",");

                if (values.Length == 4)
                {
                    if (values[3].ToLower() == "false")
                        isComplete = false;
                    else
                        isComplete = true;

                    tempTask = new Task(values[0], values[1], values[2], isComplete);

                    task_list.Add(tempTask);
                }
                else
                {
                    Console.Clear();

                    for(int I = 0; I < 4; I++)
                        Console.WriteLine("\n*** WARNING: SAVE FILE CORRUPTED, SAVED DATA LOST! ***");

                    pause();

                    File.Delete(save_data_path);

                    Console.Clear();

                    break;
                }
            }
        }

        public void updateSaveFile()
        {
            if(task_list.Count > 0)
            {
                File.Delete(save_data_path);

                foreach(Task task_element in task_list)
                    saveTask(task_element);
            }
        }

        public void saveTask(Task task_to_save)
        {
            string new_data = task_to_save.getName() + "," + task_to_save.getDate() + "," + task_to_save.getNotes() + "," + task_to_save.isComplete();

            File.AppendAllText(save_data_path, new_data + Environment.NewLine);
        }

        public void addTask()
        {
            Console.Clear();

            Console.WriteLine("/------------------\\");
            Console.WriteLine("|                  |");
            Console.WriteLine("|     ADD TASK     |");
            Console.WriteLine("|                  |");
            Console.WriteLine("\\------------------/\n");

            Console.Write("ENTER TASK NAME: ");
            string name = Console.ReadLine();

            Console.Write("ENTER TASK START DATE: ");
            string date = Console.ReadLine();

            Console.Write("ENTER TASK NOTES: ");
            string notes = Console.ReadLine();

            Task new_task = new Task(name, date, notes, false);

            task_list.Add(new_task);

            saveTask(new_task);

            Console.WriteLine("\nNew task added successfully.");

            pause();
        }

        public void editTask()
        {
            bool valid_task_num = false;
            bool valid_edit_selection = false;
            int selection;
            int edit_selection = 0;

            while (!valid_task_num)
            {
                Console.Clear();

                Console.WriteLine("/-------------------\\");
                Console.WriteLine("|                   |");
                Console.WriteLine("|     EDIT TASK     |");
                Console.WriteLine("|                   |");
                Console.WriteLine("\\-------------------/\n");

                Console.WriteLine("0. Go Back (Cancel edit)");

                for (int counter = 0; counter < task_list.Count; counter++)
                    Console.WriteLine("{0} = {1}", counter + 1, task_list[counter].getName());

                try
                {
                    Console.Write("\nWhich task number would you like to edit? ");
                    selection = Int32.Parse(Console.ReadLine());

                    if (selection > 0 && selection <= task_list.Count)
                    {
                        valid_task_num = true;

                        while (!valid_edit_selection)
                        {
                            Console.WriteLine("\nWhat would you like to edit: ");
                            Console.WriteLine("1 = Task Name");
                            Console.WriteLine("2 = Task Date");
                            Console.WriteLine("3 = Task Notes");
                            Console.WriteLine("4 = Task Complete");
                            Console.WriteLine("5 = Go Back (Cancel)\n");

                            try
                            {
                                Console.Write("Your selection? ");
                                edit_selection = Int32.Parse(Console.ReadLine());

                                if (edit_selection <= 5 && edit_selection >= 1)
                                    valid_edit_selection = true;
                                else
                                {
                                    Console.WriteLine("\n*** ERROR: Only numbers 1-5 are acceptable input ***");

                                    pause();
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("\n*** ERROR: Not acceptable input ***");

                                pause();
                            }
                        }

                        switch (edit_selection)
                        {
                            case 1:
                                Console.Write("\nENTER TASK NAME: ");
                                string name = Console.ReadLine();

                                task_list[selection - 1].setName(name);

                                break;
                            case 2:
                                Console.Write("\nENTER TASK START DATE: ");
                                string date = Console.ReadLine();

                                task_list[selection - 1].setDate(date);

                                break;
                            case 3:
                                Console.Write("\nENTER TASK NOTES: ");
                                string notes = Console.ReadLine();

                                task_list[selection - 1].setNotes(notes);

                                break;
                            case 4:
                                bool valid_user_input = false;
                                string user_input;

                                while (!valid_user_input)
                                {
                                    Console.Write("\nIs the task complete? (Y/N): ");
                                    user_input = Console.ReadLine();

                                    if (user_input.ToLower() == "y")
                                    {
                                        task_list[selection - 1].setComplete(true);
                                        valid_user_input = true;
                                    }
                                    else if (user_input.ToLower() == "n")
                                    {
                                        task_list[selection - 1].setComplete(false);
                                        valid_user_input = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine("\n*** ERROR: Only Y or N are acceptable input ***");

                                        pause();
                                    }
                                }

                                break;
                            case 5:
                                break;
                            default:
                                break;
                        }

                        if (edit_selection <= 4 && edit_selection >= 1)
                        {
                            updateSaveFile();

                            Console.WriteLine("\nThe Task Was Successfully Edited!");

                            pause();
                        }
                    }
                    else if (selection == 0)
                        valid_task_num = true;
                    else
                    {
                        Console.WriteLine("\n*** ERROR: Only numbers 0-{0} are allowed as input ***", task_list.Count);

                        pause();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("\n*** ERROR: Not acceptable input ***");

                    pause();
                }
            }
        }

        public void displayTasks()
        {
            string inspect_input;
            int task_index_input;

            bool exit_method = false;
            bool valid_inspect_input;
            bool valid_index;

            while (!exit_method)
            {
                Console.Clear();

                valid_inspect_input = false;
                valid_index = false;

                Console.WriteLine("/-------------------\\");
                Console.WriteLine("|                   |");
                Console.WriteLine("|     TASK LIST     |");
                Console.WriteLine("|                   |");
                Console.WriteLine("\\-------------------/\n");

                if (task_list.Count == 0)
                {
                    Console.WriteLine("No tasks to display.");

                    pause();

                    exit_method = true;
                }
                else
                {
                    for(int counter = 0; counter < task_list.Count; counter++)
                        Console.WriteLine("{0}. {1}", counter + 1, task_list[counter].getName());

                    while (!valid_inspect_input)
                    {
                        Console.Write("\nWOULD YOU LIKE TO INSPECT A TASK? (Y/N): ");
                        inspect_input = Console.ReadLine();

                        if (inspect_input.ToLower() == "n")
                        {
                            exit_method = true;
                            valid_inspect_input = true;
                        }
                        else if(inspect_input.ToLower() == "y")
                        {
                            valid_inspect_input = true;

                            while (!valid_index)
                            {
                                try
                                {
                                    Console.Write("\nWHAT TASK NUMBER WOULD YOU LIKE TO INSPECT? ");
                                    task_index_input = Int32.Parse(Console.ReadLine());

                                    // checks if the user inputs a task number that's out of bounds
                                    if (task_index_input > task_list.Count || task_index_input < 1)
                                        Console.WriteLine("\n*** ERROR: Only numbers 1-{0} are acceptable input ***", task_list.Count);
                                    else
                                    {
                                        valid_index = true;

                                        Console.WriteLine("\nTASK INFO:");
                                        Console.WriteLine("----------");
                                        Console.WriteLine("\tNAME: {0}\n\tSTART DATE: {1}\n\tTASK COMPLETE? {2}\n\tTASK NOTES: {3}", task_list[task_index_input - 1].getName(), task_list[task_index_input - 1].getDate(),
                                            task_list[task_index_input - 1].isComplete(), task_list[task_index_input - 1].getNotes());

                                        pause();
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("\n*** ERROR: Not acceptable input ***");
                                }
                            }
                        }
                        else
                            Console.WriteLine("\n*** ERROR: Input must be either Y or N ***");
                    }
                }
            }
        }

        public void removeTask()
        {
            Console.Clear();

            Console.WriteLine("/---------------------\\");
            Console.WriteLine("|                     |");
            Console.WriteLine("|     REMOVE TASK     |");
            Console.WriteLine("|                     |");
            Console.WriteLine("\\---------------------/\n");

            if (task_list.Count > 0)
            {
                bool valid_index = false;

                Console.WriteLine("0 = Go Back (Cancel Remove)");

                for (int counter = 0; counter < task_list.Count; counter++)
                    Console.WriteLine("{0} = {1}", counter + 1, task_list[counter].getName());

                while (!valid_index)
                {
                    try
                    {
                        Console.Write("\nENTER A TASK NUMBER YOU WISH TO REMOVE: ");
                        int task_index_input = Int32.Parse(Console.ReadLine());

                        if (task_index_input < 0 || task_index_input > task_list.Count)
                            Console.WriteLine("\n*** ERROR: Input must be between 1-{0} ***", task_list.Count);
                        else
                        {
                            valid_index = true;

                            if (task_index_input != 0)
                            {
                                task_list.RemoveAt(task_index_input - 1);

                                updateSaveFile();

                                Console.WriteLine("\nTask removed successfully.");

                                pause();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("\n*** ERROR: Not acceptable input ***");
                    }
                }
            }
            else
            {
                Console.WriteLine("No tasks to remove, to-do list is empty.");

                pause();
            }
        }
    }

    ////////////////////////////////////////////////////////////////
    ///
    /// TASK CLASS
    /// 
    ////////////////////////////////////////////////////////////////

    class Task
    {
        string task_name = "empty";
        string task_date = "empty";
        string task_notes = "empty";
        bool task_complete;

        public Task(string name, string date, string notes, bool complete)
        {
            task_name = name;
            task_date = date;
            task_notes = notes;
            task_complete = complete;
        }

        public string getName() { return task_name; }

        public string getDate() { return task_date; }

        public string getNotes() { return task_notes; }

        public bool isComplete() { return task_complete; }

        public void setName(string name) { task_name = name; }

        public void setDate(string date) { task_date = date; }

        public void setNotes(string notes) { task_notes = notes; }

        public void setComplete(bool complete) { task_complete = complete; }
    }

    /////////////////////////////////////////////////////////////
    /// 
    /// PROGRAM CLASS
    /// 
    /////////////////////////////////////////////////////////////

    class Program
    {
        static int menu()
        {
            int menu_option = 0;
            bool exit_menu = false;

            while (!exit_menu)
            {
                try
                {
                    Console.Clear();

                    Console.WriteLine("/-------------------\\");
                    Console.WriteLine("|                   |");
                    Console.WriteLine("|     MAIN MENU     |");
                    Console.WriteLine("|                   |");
                    Console.WriteLine("\\-------------------/\n");

                    Console.WriteLine("1 = ADD NEW TASK\n");
                    Console.WriteLine("2 = REMOVE TASK\n");
                    Console.WriteLine("3 = VIEW TASKS\n");
                    Console.WriteLine("4 = EDIT TASK\n");
                    Console.WriteLine("5 = EXIT");

                    Console.Write("\nENTER A MENU NUMBER SELECTION: ");
                    menu_option = Int32.Parse(Console.ReadLine());

                    if (menu_option > 5 || menu_option < 1)
                    {
                        Console.WriteLine("\n*** ERROR: Input must be numbers 1-5 ***");

                        Console.WriteLine("\n*** Press [ENTER] to continue ***");
                        Console.ReadLine();
                    }
                    else
                        exit_menu = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("\n*** ERROR: Not acceptable input ***");

                    Console.WriteLine("\n*** Press [ENTER] to continue ***");
                    Console.ReadLine();
                }
            }

            return menu_option;
        }

        static void Main(string[] args)
        {
            ToDoList toDoApp = new ToDoList();

            int menu_selection;
            bool exit_program = false;

            while (!exit_program)
            {
                menu_selection = menu();

                switch(menu_selection)
                {
                    case 1:
                        toDoApp.addTask();
                        break;
                    case 2:
                        toDoApp.removeTask();
                        break;
                    case 3:
                        toDoApp.displayTasks();
                        break;
                    case 4:
                        toDoApp.editTask();
                        break;
                    case 5:
                        Console.WriteLine("\nThanks for using the To-Do List. See ya next time! Press [ENTER] to exit.");
                        Console.ReadLine();
                        exit_program = true;
                        break;
                    default:
                        Console.WriteLine("\n***ERROR: Menu selection is out of bounds ***\n");
                        Console.Write("*** Press [ENTER] to continue ***");
                        Console.ReadLine();
                        break;
                }
            }
        }
    }
}
