using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CMP1124M_Assignment_2
{
    // Class for holding the arrays to be used across the classes
    public static class Globals
    {
        // Load each of the resources into their designated array
        public static double[] High_256 = FileSystem.load_resource(Properties.Resources.High_256);
        public static double[] High_4096 = FileSystem.load_resource(Properties.Resources.High_4096);
        public static double[] High_2048 = FileSystem.load_resource(Properties.Resources.High_2048);
        public static double[] Low_256 = FileSystem.load_resource(Properties.Resources.Low_256);
        public static double[] Low_2048 = FileSystem.load_resource(Properties.Resources.Low_2048);
        public static double[] Low_4096 = FileSystem.load_resource(Properties.Resources.Low_4096);
        public static double[] Mean_2048 = FileSystem.load_resource(Properties.Resources.Mean_2048);
        public static double[] Mean_256 = FileSystem.load_resource(Properties.Resources.Mean_256);
        public static double[] Mean_4096 = FileSystem.load_resource(Properties.Resources.Mean_4096);
    }

    // Main class for the application
    class Program
    {
        // Entry point for the application
        static void Main(string[] args)
        {
            // Welcome welcome welcome
            Console.WriteLine("Please choose one of the following:");
            Console.WriteLine("3 - High 256");
            Console.WriteLine("2 - Mean 256");
            Console.WriteLine("1 - Low 256");
            Console.Write("Data set: ");
            // Record the user input, try to parse to an int, if it can't, keep asking
            int user_input = Interaction.input_validation(Console.ReadLine(), 3);
            // Clear the console
            Console.Clear();
            // Call out to the function to handle the sort request
            Interaction.Arrays_Sorted Arrays_Sorted = Interaction.run_sort(user_input, false);
            // Wait for the user to press a key
            Console.WriteLine();
            Console.Write("Press any button to continue");
            Console.ReadLine();
            Console.Clear();
            // Ask politely what value the user would hope to find in the array
            Console.Write("What value do you want to search for?");
            double user_selection = Interaction.entry_validation(Console.ReadLine());
            // Once the input has been validated, run the search functionality
            // Searching.value_search(user_selection, Arrays_Sorted.ascending_sorted);  // <-- First iteration of code saw the use of a linear search over binary
            Searching.binary_search_alg(user_selection, Arrays_Sorted.ascending_sorted);
            // Now we move onto the bigger files
            Console.WriteLine("Now for the larger data sets:");
            Console.WriteLine("1 - Low 2048");
            Console.WriteLine("2 - Low 4096");
            Console.WriteLine("3 - Mean 2048");
            Console.WriteLine("4 - Mean 4096");
            Console.WriteLine("5 - High 2048");
            Console.WriteLine("6 - High 4096");
            Console.Write("Data set: ");
            // Record the user input, try to parse to an int, if it can't, keep asking
            user_input = Interaction.input_validation(Console.ReadLine(), 6);
            // Clear the console
            Console.Clear();
            // Call out to the function to handle the sort request
            Interaction.run_sort(user_input, true);
            // Wait for the user to press a key
            Console.WriteLine();
            Console.Write("Press any button to continue");
            Console.ReadLine();
            Console.Clear();
            // Ask politely what value the user would hope to find in the array
            Console.Write("Enter a value to search for");
            user_selection = Interaction.entry_validation(Console.ReadLine());
            // Once the input has been validated, run the search functionality
            Arrays_Sorted = Interaction.run_sort(user_input, true);
            // Searching.value_search(user_selection, Interaction.chosen_array(user_input, true));  // <-- First iteration of code saw the use of a linear search over binary
            Searching.binary_search_alg(user_selection, Arrays_Sorted.ascending_sorted);
            // Loop 3 times to print out the merged results of each array size
            for (int data_set = 0; data_set < 3; data_set = data_set + 1)
            {
                // Run the sort on the merge of the arrays
                Arrays_Sorted = Interaction.array_sort(Interaction.array_merging(data_set));
                // Run the search functionality
                Interaction.handle_search(Arrays_Sorted.ascending_sorted);
            }
            // Ask if they want to restart
            Console.Write("Would you like to go again?(y/n)");
            if (Console.ReadLine() == "y")
            {
                // Clear the console
                Console.Clear();
                // Call the main function to run again
                Main(null);
            }
            // Close the program down
            Environment.Exit(0);
        }
    }

    // Class for handling all requests to the project resources
    class FileSystem
    {
        // Load in the specified resource file, returning an array of integers
        public static double[] load_resource(string resource)
        {
            // Use a try catch method to load the resource into an array
            try
            {
                // Split the resource file into an array of strings
                string[] array_version = resource.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                // Define a list to be added to during the following iteration over the new array
                List<double> list_format = new List<double>();
                // Loop over each element in the array
                foreach (string element in array_version)
                {
                    // Try parse the element to a double
                    double number;
                    if (Double.TryParse(element, out number))
                    {
                        // If it can be successfully parsed, add it to the list
                        list_format.Add(number);
                    }
                }
                // Convert the list back into an array of doubles
                double[] return_array = list_format.ToArray();
                // Return the array
                return return_array;
            }
            // If we encounter an error, print it to console for debugging
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                Console.ReadLine();
                Environment.Exit(0);
                return null;
            }
        }
    }

    // Class for handling user interaction such as validating inputs
    class Interaction
    {
        // Validate the user input to make sure it's an integer and within the boundries
        public static int input_validation(string raw, int max)
        {
            // Take in the user input as a string titled 'raw'
            int validated = 0;
            int input;
            // Try to parse the input first to an integer
            while ((Int32.TryParse(raw, out input) == false) || (input > max) || (input < 1))
            {
                // Tell the user off for trying to be clever and take their next input
                Console.Write("Please enter a valid option: ");
                raw = Console.ReadLine();
            }
            validated = input;
            // Return the validated input
            return validated;
        }

        // Validate the input for searching through the array with
        public static double entry_validation(string raw)
        {
            // Take in the user input as a string titled 'raw'
            double validated = 0;
            double input;
            // Try to parse the input first to an integer
            while (Double.TryParse(raw, out input) == false)
            {
                // Tell the user off for trying to be clever and take their next input
                Console.Write("Please enter a valid option");
                raw = Console.ReadLine();
            }
            validated = input;
            // Return the validated input
            return validated;
        }

        // Create a struct to returnt he sorted arrays
        public struct Arrays_Sorted
        {
            // Store 2 arrays, one for ascending order and one for descending order
            public double[] ascending_sorted;
            public double[] descending_sorted;
        }

        // Take the user input and run the appropriate sort function
        public static Arrays_Sorted run_sort(int selection, bool large_data_set)
        {
            // Return the sorted arrays through a struct
            Arrays_Sorted return_vals = new Arrays_Sorted();
            // Firstly, congratulate the user on a good selection
            // Run through a switch case to see which array we need to target













            double[] target = chosen_array(selection, large_data_set);
            // Check to see what iterations we need to print out on
            int iterations = Sorting.array_size_finder(target);
            // Make a call to the sorting class to run a binary in order traversal
            // Sorting.bubble_asc(target, false);  // <-- First iteration of code saw the use of a bubble sort over binary
            BST.Tree_Info bst_tree = BST.init_tree(target);
            BST.Tree binary_tree = bst_tree.binary_tree;
            // Clear the values currently held
            Sorting.sorted_ascending_array.Clear();
            Sorting.sorted_descending_array.Clear();
            Console.WriteLine(Environment.NewLine + "Ascending order {0}th value:", iterations);
            // Sort the incoming array using a binary tree structure
            Sorting.Binary_st_ascending(bst_tree.curr_root);
            // Convert the returned list to an array
            double[] ascending_sorted = Sorting.sorted_ascending_array.ToArray();
            // Print the values we need to the console
            display_values(ascending_sorted, iterations, large_data_set);
            return_vals.ascending_sorted = ascending_sorted;
            // Then make a call to the sorting class to run a binary  reverse in order traversal
            // Sorting.bubble_dsc(target, false);  // <-- First iteration of code saw the use of a bubble sort over binary
            Console.WriteLine(Environment.NewLine + "Desending order {0}th value:", iterations);
            // Sort the incoming array using a binary tree structure
            Sorting.Binary_st_descending(bst_tree.curr_root);
            // Convert the returned list to an array
            double[] descending_sorted = Sorting.sorted_descending_array.ToArray();
            // Print the values we need to the console
            display_values(descending_sorted, iterations, large_data_set);
            return_vals.descending_sorted = descending_sorted;
            // Return the sorted arrays
            return return_vals;
        }

        // Take in an array to sort instead of fetching it
        public static Arrays_Sorted array_sort(double[] target)
        {
            // Make a call to the sorting class to run a bubble sort in ascending order
            // Sorting.bubble_asc(target, false);  // <- First version of code used the bubble sort method
            // Then make a call to the sorting class to run a bubble sort in ascending order
            // Sorting.bubble_dsc(target, false);  // <- First version of code used the bubble sort method

            // Store the arrays for returning
            Arrays_Sorted return_vals = new Arrays_Sorted();
            // We want to print out every 10th value
            int iterations = 10;
            // Make a call to the sorting class to run a binary in order traversal
            BST.Tree_Info direct_bst_tree = BST.init_tree(target);
            BST.Tree binary_tree = direct_bst_tree.binary_tree;
            // Clear the values currently held in the lists
            Sorting.sorted_ascending_array.Clear();
            Sorting.sorted_descending_array.Clear();
            Console.WriteLine(Environment.NewLine + "Ascending order {0}th value:", iterations);
            // Sort the incoming array using a binary tree structure
            Sorting.Binary_st_ascending(direct_bst_tree.curr_root);
            // Convert the returned list to an array
            double[] ascending_sorted = Sorting.sorted_ascending_array.ToArray();
            // Print the values we need to the console
            display_values(ascending_sorted, iterations, false);
            return_vals.ascending_sorted = ascending_sorted;
            Console.WriteLine(Environment.NewLine + "Desending order {0}th value:", iterations);
            // Sort the incoming array using a binary tree structure
            Sorting.Binary_st_descending(direct_bst_tree.curr_root);
            // Convert the returned list to an array
            double[] descending_sorted = Sorting.sorted_descending_array.ToArray();
            // Print the values we need to the console
            display_values(descending_sorted, iterations, false);
            return_vals.descending_sorted = descending_sorted;
            return return_vals;
        }

        // Print the values we need to the console
        public static void display_values(double[] chosen_array, int iterations, bool large_data_set)
        {
            // Are we looking at the bigger datasets?
            if (large_data_set)
            {
                // In that case, only print out the value found once instead of every iteration
                Console.WriteLine("At index {0} we have {1}", iterations, chosen_array[iterations]);
            }
            // Otherwise
            else
            {
                // Loop over the sorted array and output the values we need
                for (int i = 0; i < chosen_array.Length; i += iterations)
                {
                    // Print out the values seen at every 10 place
                    Console.WriteLine("At index {0} we have {1}", i, chosen_array[i]);
                }
            }
        }

        // Return the array at the of the given selection
        public static double[] chosen_array(int selection, bool large_data_set)
        {
            // Run through a switch case to see which array we need to return
            double[] chosen_data = null;
            // Are we looking at the bigger data sets?
            if (large_data_set == false)
            {
                // Return the smaller data sets
                switch (selection)
                {
                    case 1:
                        chosen_data = Globals.Low_256;
                        break;
                    case 2:
                        chosen_data = Globals.Mean_256;
                        break;
                    case 3:
                        chosen_data = Globals.High_256;
                        break;
                }
            }
            else
            {
                // Return the bigger data sets
                switch (selection)
                {
                    case 1:
                        chosen_data = Globals.Low_2048;
                        break;
                    case 2:
                        chosen_data = Globals.Low_4096;
                        break;
                    case 3:
                        chosen_data = Globals.Mean_2048;
                        break;
                    case 4:
                        chosen_data = Globals.Mean_4096;
                        break;
                    case 5:
                        chosen_data = Globals.High_2048;
                        break;
                    case 6:
                        chosen_data = Globals.High_4096;
                        break;
                }
            }
            // Return the chosen array
            return chosen_data;
        }

        // Merge 2 given arrays using the given size
        public static double[] array_merging(int array_size_finder)
        {
            // Setup some storage for our selected array
            double[] upper_arr = null;
            double[] lower_arr = null;
            int size = 0;
            // Utilize a switch case to select our targeted arrays
            switch (array_size_finder)
            {
                case 0:
                    lower_arr = Globals.Low_256;
                    upper_arr = Globals.High_256;
                    size = 256;
                    break;
                case 1:
                    lower_arr = Globals.Low_2048;
                    upper_arr = Globals.High_2048;
                    size = 2048;
                    break;
                case 2:
                    lower_arr = Globals.Low_4096;
                    upper_arr = Globals.High_4096;
                    size = 4096;
                    break;
            }
            // See if we need to print out a message to the console
            Console.WriteLine("Output of the merge of the Low and High {0} arrays:", size);
            // Create a new array with the size of both arrays together
            double[] merged_array = new double[lower_arr.Length + upper_arr.Length];
            // Copy in the two arrays
            Array.Copy(lower_arr, merged_array, lower_arr.Length);
            Array.Copy(upper_arr, 0, merged_array, lower_arr.Length, upper_arr.Length);
            // Return the merged array
            return merged_array;
        }

        // Handle the searching for merged arrays
        public static void handle_search(double[] merged_array)
        {
            // Wait for the user to press a key
            Console.WriteLine();
            Console.Write("Press any button to continue");
            Console.ReadLine();
            Console.Clear();
            // Ask politely what value the user would hope to find in the array
            Console.Write("enter a value you want to search for: ");
            double user_selection = Interaction.entry_validation(Console.ReadLine());
            // Once the input has been validated, run the search functionality
            // Searching.value_search(user_selection, merged_array);
            Searching.binary_search_alg(user_selection, merged_array);
        }
    }

    // Class for handling the different sorting functions
    class Sorting
    {
        // Create a list element for holding the sorted array
        public static List<double> sorted_ascending_array = new List<double>();
        public static List<double> sorted_descending_array = new List<double>();

        // Find the size of the given array
        public static int array_size_finder(double[] input_array)
        {
            // Return the given increments depending on the size of the array passed in
            switch (input_array.Length)
            {
                case 256:
                    return 10;
                case 2048:
                    return 50;
                case 4096:
                    return 80;
                default:
                    return 10;
            }
        }
        // In order traversal for the binary tree
        public static void Binary_st_ascending(BST.Node root)
        {
            // If we are at the end
            if (root == null)
            {
                // Return null
                return;
            }
            // Follow down the left path of the node
            Binary_st_ascending(root.left);
            // Add the value to the list to be returned
            sorted_ascending_array.Add(root.value);
            // Then traverse down the right side of the root
            Binary_st_ascending(root.right);
        }

        // In order traversal for the binary tree
        public static void Binary_st_descending(BST.Node root)
        {
            // If we are at the end
            if (root == null)
            {
                // Return null
                return;
            }
            // Then traverse down the right side of the root
            Binary_st_descending(root.right);
            // Add the value to the list to be returned
            sorted_descending_array.Add(root.value);
            // Follow down the left path of the node
            Binary_st_descending(root.left);
        }
    }

    // Class for handling the searching functionality
    class Searching
    {
        // Linear search for specified value
        public static void value_search(double request, double[] in_array)
        {
            // Declare a list for storing the locations it was found at
            List<int> indexes = new List<int>();
            // Declare a boolean to catch wheter we found the number or not
            bool found = false;
            // Count the operations needed
            // Loop over the entire array in search for the golden number
            for (int position = 0; position < in_array.Length - 1; position = position + 1)
            {
                // First check the element to see if we can find a direct match
                if (request == in_array[position])
                {
                    // We found it cap!
                    found = true;
                    // Add it to the indexes list for looping over in a sec
                    indexes.Add(position);
                }
            }
            // Inform the user the search is being done on a sorted array
            Console.WriteLine("The seach done over the sorted array in ascending order returned these results:");
            // If the number was found, print out the locations it was found at
            if (found)
            {
                // Conver the list to an array and loop over it
                int[] indexes_arr = indexes.ToArray();
                foreach (int index in indexes_arr)
                {
                    // Print to console the position the number was found at
                    Console.WriteLine("Found ya number at index {0}", index);
                }
            }
            // If we couldn't find the exact number, find the closest one
            else
            {
                int closest_index = 0;
                double closest_value = 0;
                double difference = 0;
                // Loop over the array
                for (int iteration = 0; iteration < in_array.Length - 1; iteration = iteration + 1)
                {
                    // Work out the difference between the two numbers
                    double diff = request - in_array[iteration];
                    // If the result is less than 0, multiply it up to keep things easy
                    if (diff < 0)
                    {
                        diff *= -1;
                    }
                    // Is this difference smaller than the one we have? (is it closer to the requested value)
                    if ((diff < difference) || (difference == 0))
                    {
                        // If it is, then set our variables accordingly
                        closest_index = iteration;
                        closest_value = in_array[iteration];
                        difference = diff;
                    }
                }
                // Let the user down lightly
                Console.WriteLine("Sorry mate, couldn't find your num. But {0} at index {1} is pretty close.", closest_value, closest_index);
                Console.Write(Environment.NewLine + "Alright mate no worries, thanks for looking. (press any key)");
                // Console.ReadLine();
                // Console.Clear();
            }
        }

        // Struct for holding info about the closest nodes in case of the request not being found
        struct Unfound_Replacement
        {
            // Hold a value and an index
            public double value;
            public int index;
        }

        // Binary search alternative
        public static void binary_search_alg(double request, double[] in_array)
        {
            // Clear the console (keep it clean)
            Console.Clear();
            // Have we found the request the user was looking for?
            bool found = false;
            // Upper and lower bounds of our search params
            int lower_bound = 0;
            int upper_bound = in_array.Length;
            // Hold the closest numbers both smaller and bigger
            Unfound_Replacement closest_smaller = new Unfound_Replacement();
            Unfound_Replacement closest_bigger = new Unfound_Replacement();
            // Count the amount of operations it takes
            int operations = 0;
            // As it says below (while our binary search is still active)
            while (lower_bound < upper_bound)
            {
                // Calculate the mid point
                int mid = (lower_bound + upper_bound) / 2;
                // If this is our request, nice!
                if (request == in_array[mid])
                {
                    // Write it to the console
                    Console.WriteLine("Sorted in ascending order, we found {0} at index {1}.", request, mid);
                    found = true;
                    // Save the current index for the mid point
                    int index = mid;
                    // If the element to the left of the current index also matches
                    if (in_array[mid - 1] == request)
                    {
                        // Begin a while loop to iterate over every instance found
                        while (in_array[index - 1] == request)
                        {
                            // Subtract 1 from the index value
                            index = index - 1;
                            // Print out that the value was also found at this location
                            Console.WriteLine("In addition, in ascending order we found {0} at index {1}.", request, index);
                            // Increment the operations by 1
                            operations = operations + 1;
                        }
                    }
                    // Reset the index to the midpoint again
                    index = mid;
                    // If the element to the right of the current index also matches
                    if (in_array[mid + 1] == request)
                    {
                        // Begin a while loop to iterate over every instance found
                        while (in_array[index + 1] == request)
                        {
                            // Increment 1 to the index value
                            index = index + 1;
                            // Print out that the value was also found at this location
                            Console.WriteLine("Sorted in ascending order, we found {0} at index {1}.", request, index);
                            // Increment the operations by 1
                            operations = operations + 1;
                        }
                    }
                    // Print out the total operations the search took
                    Console.WriteLine();
                    Console.WriteLine("This took {0} operations to complete.", operations);
                    // Break ot of the loop
                    break;
                }
                // If the request is left than this position
                else if (request < in_array[mid])
                {
                    // Adjust the boundries
                    upper_bound = mid - 1;
                    // Save the details of this node in case this is the last iteration
                    closest_bigger.value = in_array[mid];
                    closest_bigger.index = mid;
                }
                // Otherwise if it's bigger
                else
                {
                    // Adjust the boundries again
                    lower_bound = mid + 1;
                    // Save the details of this node in case this is the last iteration
                    closest_smaller.value = in_array[mid];
                    closest_smaller.index = mid;
                }
                // Iterate the amount of operations
                operations = operations + 1;
            }
            // If the request was not found
            if (!found)
            {
                // Work out the difference between the request and the closest smaller number
                double diff_sm = request - closest_smaller.value;
                // If the result is less than 0, multiply it up to keep things easy
                if (diff_sm < 0)
                {
                    diff_sm *= -1;
                }
                // Work out the difference between the request and the closest bigger number
                double diff_bg = request - closest_bigger.value;
                // If the result is less than 0, multiply it up to keep things easy
                if (diff_bg < 0)
                {
                    diff_bg *= -1;
                }
                // Print out the operations it's taken
                Console.WriteLine("Search took {0} operations before coming to the following conclusion:", operations);
                //  Work out which difference is smaller
                if (diff_bg < diff_sm)
                {
                    // If the the bigger number is closer, print that out
                    Console.WriteLine("Couldn't find your {3}, but {0} at index {1} is only {2} away.", closest_bigger.value, closest_bigger.index, diff_bg, request);
                }
                else
                {
                    // Otherwise print out the smaller number
                    Console.WriteLine("Couldn't find your {3}, but {0} at index {1} is only {2} away.", closest_smaller.value, closest_smaller.index, diff_sm, request);
                }
                // Wait for the user's input before continuing
                Console.WriteLine(Environment.NewLine + "No worries mate, thanks for looking. (press any key to continue)");
                Console.ReadLine();
                Console.Clear();
            }
            // If it was found
            else
            {
                // Just wait for the users input
                Console.WriteLine(Environment.NewLine + "Press any key to continue...");
                Console.ReadLine();
                Console.Clear();
            }
        }
    }

    // Bolt-on class for handling searching and sorting through the use of a binary tree
    class BST
    {
        // Create a struct for returning some data
        public struct Tree_Info
        {
            public Tree binary_tree;
            public Node curr_root;
        }

        // Initialize the binary search tree
        public static Tree_Info init_tree(double[] target_arr)
        {
            // Create new instances of the root and tree
            Node root = null;
            Tree binary_tree = new Tree();
            // Loop over the data set and perform insersions into the tree
            for (int i = 0; i < target_arr.Length; i = i +1)
            {
                root = binary_tree.insert(root, target_arr[i]);
            }
            // Return the information
            Tree_Info tree_data = new Tree_Info();
            tree_data.binary_tree = binary_tree;
            tree_data.curr_root = root;
            return tree_data;
        }

        // Node class to hold current value as well as left and right values
        public class Node
        {
            // A value for the node along with left and right nodes
            public double value;
            public Node left;
            public Node right;
        }

        // Tree class handling functions such as inserting and traversing
        public class Tree
        {
            // Inserting a value takes in a root value and a value to add
            public Node insert(Node root, double element)
            {
                // If it's the first element in
                if (root == null)
                {
                    // Create a new node and then assign it's value to the one passes in
                    root = new Node();
                    root.value = element;
                }
                // If the value is less than the root given
                else if (element < root.value)
                {
                    // Apply the value to the left side of the node
                    root.left = insert(root.left, element);
                }
                // Otherwise (in this case if it's greater)
                else
                {
                    // Apply the value to the right of the node
                    root.right = insert(root.right, element);
                }
                // Return back the root node given
                return root;
            }
        }
    }
}