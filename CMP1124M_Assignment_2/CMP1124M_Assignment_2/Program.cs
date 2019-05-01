using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CMP1124M_Assignment_2
{
    public static class Globals
    {
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

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please choose one of the following:");
            Console.WriteLine("3 - High 256");
            Console.WriteLine("2 - Mean 256");
            Console.WriteLine("1 - Low 256");
            Console.Write("Data set: ");
            int user_input = Interaction.input_validation(Console.ReadLine(), 3);
            Console.Clear();
            Interaction.Arrays_Sorted Arrays_Sorted = Interaction.run_sort(user_input, false);
            Console.WriteLine();
            Console.Write("Press any button to continue");
            Console.ReadLine();
            Console.Clear();
            Console.Write("What value do you want to search for?");
            double user_selection = Interaction.entry_validation(Console.ReadLine());
            Searching.binary_search_alg(user_selection, Arrays_Sorted.ascending_sorted);
            Console.WriteLine("Now for the larger data sets:");
            Console.WriteLine("1 - Low 2048");
            Console.WriteLine("2 - Low 4096");
            Console.WriteLine("3 - Mean 2048");
            Console.WriteLine("4 - Mean 4096");
            Console.WriteLine("5 - High 2048");
            Console.WriteLine("6 - High 4096");
            Console.Write("Data set: ");
            user_input = Interaction.input_validation(Console.ReadLine(), 6);
            Console.Clear();
            Interaction.run_sort(user_input, true);
            Console.WriteLine();
            Console.Write("Press any button to continue");
            Console.ReadLine();
            Console.Clear();
            Console.Write("Enter a value to search for");
            user_selection = Interaction.entry_validation(Console.ReadLine());
            Arrays_Sorted = Interaction.run_sort(user_input, true);
            Searching.binary_search_alg(user_selection, Arrays_Sorted.ascending_sorted);
            for (int data_set = 0; data_set < 3; data_set = data_set + 1)
            {
                Arrays_Sorted = Interaction.array_sort(Interaction.array_merging(data_set));
                Interaction.handle_search(Arrays_Sorted.ascending_sorted);
            }
            Console.Write("Would you like to go again?(y/n)");
            if (Console.ReadLine() == "y")
            {
                Console.Clear();
                Main(null);
            }
            Environment.Exit(0);
        }
    }

    class FileSystem
    {
        public static double[] load_resource(string resource)
        {
            try
            {
                string[] array_version = resource.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                List<double> list_format = new List<double>();
                foreach (string element in array_version)
                {
                    double number;
                    if (Double.TryParse(element, out number))
                    {
                        list_format.Add(number);
                    }
                }
                double[] return_array = list_format.ToArray();
                return return_array;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                Console.ReadLine();
                Environment.Exit(0);
                return null;
            }
        }
    }

    class Interaction
    {
        public static int input_validation(string raw, int max)
        {
            int validated = 0;
            int input;
            while ((Int32.TryParse(raw, out input) == false) || (input > max) || (input < 1))
            {
                Console.Write("Please enter a valid option: ");
                raw = Console.ReadLine();
            }
            validated = input;
            return validated;
        }

        public static double entry_validation(string raw)
        {
            double validated = 0;
            double input;
            while (Double.TryParse(raw, out input) == false)
            {
                Console.Write("Please enter a valid option");
                raw = Console.ReadLine();
            }
            validated = input;
            return validated;
        }

        public struct Arrays_Sorted
        {
            public double[] ascending_sorted;
            public double[] descending_sorted;
        }

        public static Arrays_Sorted run_sort(int selection, bool large_data_set)
        {
            Arrays_Sorted return_vals = new Arrays_Sorted();
            double[] target = chosen_array(selection, large_data_set);
            int iterations = Sorting.array_size_finder(target);
            BST.Tree_Info bst_tree = BST.init_tree(target);
            BST.Tree binary_tree = bst_tree.binary_tree;
            Sorting.sorted_ascending_array.Clear();
            Sorting.sorted_descending_array.Clear();
            Console.WriteLine(Environment.NewLine + "Ascending order {0}th value:", iterations);
            Sorting.Binary_st_ascending(bst_tree.curr_root);
            double[] ascending_sorted = Sorting.sorted_ascending_array.ToArray();
            display_values(ascending_sorted, iterations, large_data_set);
            return_vals.ascending_sorted = ascending_sorted;
            Console.WriteLine(Environment.NewLine + "Desending order {0}th value:", iterations);
            Sorting.Binary_st_descending(bst_tree.curr_root);
            double[] descending_sorted = Sorting.sorted_descending_array.ToArray();
            display_values(descending_sorted, iterations, large_data_set);
            return_vals.descending_sorted = descending_sorted;
            return return_vals;
        }

        public static Arrays_Sorted array_sort(double[] target)
        {
            Arrays_Sorted return_vals = new Arrays_Sorted();
            int iterations = 10;
            BST.Tree_Info direct_bst_tree = BST.init_tree(target);
            BST.Tree binary_tree = direct_bst_tree.binary_tree;
            Sorting.sorted_ascending_array.Clear();
            Sorting.sorted_descending_array.Clear();
            Console.WriteLine(Environment.NewLine + "Ascending order {0}th value:", iterations);
            Sorting.Binary_st_ascending(direct_bst_tree.curr_root);
            double[] ascending_sorted = Sorting.sorted_ascending_array.ToArray();
            display_values(ascending_sorted, iterations, false);
            return_vals.ascending_sorted = ascending_sorted;
            Console.WriteLine(Environment.NewLine + "Desending order {0}th value:", iterations);
            Sorting.Binary_st_descending(direct_bst_tree.curr_root);
            double[] descending_sorted = Sorting.sorted_descending_array.ToArray();
            display_values(descending_sorted, iterations, false);
            return_vals.descending_sorted = descending_sorted;
            return return_vals;
        }

        public static void display_values(double[] chosen_array, int iterations, bool large_data_set)
        {
            if (large_data_set)
            {
                Console.WriteLine("At index {0} we have {1}", iterations, chosen_array[iterations]);
            }
            else
            {
                for (int i = 0; i < chosen_array.Length; i += iterations)
                {
                    Console.WriteLine("At index {0} we have {1}", i, chosen_array[i]);
                }
            }
        }

        public static double[] chosen_array(int selection, bool large_data_set)
        {
            double[] chosen_data = null;
            if (large_data_set == false)
            {
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
            return chosen_data;
        }

        public static double[] array_merging(int array_size_finder)
        {
            double[] upper_arr = null;
            double[] lower_arr = null;
            int size = 0;
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
            Console.WriteLine("Output of the merge of the Low and High {0} arrays:", size);
            double[] merged_array = new double[lower_arr.Length + upper_arr.Length];
            Array.Copy(lower_arr, merged_array, lower_arr.Length);
            Array.Copy(upper_arr, 0, merged_array, lower_arr.Length, upper_arr.Length);
            return merged_array;
        }

        public static void handle_search(double[] merged_array)
        {
            Console.WriteLine();
            Console.Write("Press any button to continue");
            Console.ReadLine();
            Console.Clear();
            Console.Write("enter a value you want to search for: ");
            double user_selection = Interaction.entry_validation(Console.ReadLine());
            Searching.binary_search_alg(user_selection, merged_array);
        }
    }

    class Sorting
    {
        public static List<double> sorted_ascending_array = new List<double>();
        public static List<double> sorted_descending_array = new List<double>();

        public static int array_size_finder(double[] input_array)
        {
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
        public static void Binary_st_ascending(BST.Node root)
        {
            if (root == null)
            {
                return;
            }
            Binary_st_ascending(root.left);
            sorted_ascending_array.Add(root.value);
            Binary_st_ascending(root.right);
        }

        public static void Binary_st_descending(BST.Node root)
        {
            if (root == null)
            {
                return;
            }
            Binary_st_descending(root.right);
            sorted_descending_array.Add(root.value);
            Binary_st_descending(root.left);
        }
    }

    class Searching
    {
        public static void value_search(double request, double[] in_array)
        {
            List<int> indexes = new List<int>();
            bool found = false;
            for (int position = 0; position < in_array.Length - 1; position = position + 1)
            {
                if (request == in_array[position])
                {
                    found = true;
                    indexes.Add(position);
                }
            }
            Console.WriteLine("The seach done over the sorted array in ascending order returned these results:");
            if (found)
            {
                int[] indexes_arr = indexes.ToArray();
                foreach (int index in indexes_arr)
                {
                    Console.WriteLine("Found ya number at index {0}", index);
                }
            }
            else
            {
                int closest_index = 0;
                double closest_value = 0;
                double difference = 0;
                for (int iteration = 0; iteration < in_array.Length - 1; iteration = iteration + 1)
                {
                    double diff = request - in_array[iteration];
                    if (diff < 0)
                    {
                        diff *= -1;
                    }
                    if ((diff < difference) || (difference == 0))
                    {
                        closest_index = iteration;
                        closest_value = in_array[iteration];
                        difference = diff;
                    }
                }
                Console.WriteLine("Sorry mate, couldn't find your num. But {0} at index {1} is pretty close.", closest_value, closest_index);
                Console.Write(Environment.NewLine + "Alright mate no worries, thanks for looking. (press any key)");
            }
        }

        struct Unfound_Replacement
        {
            public double value;
            public int index;
        }

        public static void binary_search_alg(double request, double[] in_array)
        {
            Console.Clear();
            bool found = false;
            int lower_bound = 0;
            int upper_bound = in_array.Length;
            Unfound_Replacement closest_smaller = new Unfound_Replacement();
            Unfound_Replacement closest_bigger = new Unfound_Replacement();
            int operations = 0;
            while (lower_bound < upper_bound)
            {
                int mid = (lower_bound + upper_bound) / 2;
                if (request == in_array[mid])
                {
                    Console.WriteLine("Sorted in ascending order, we found {0} at index {1}.", request, mid);
                    found = true;
                    int index = mid;
                    if (in_array[mid - 1] == request)
                    {
                        while (in_array[index - 1] == request)
                        {
                            index = index - 1;
                            Console.WriteLine("In addition, in ascending order we found {0} at index {1}.", request, index);
                            operations = operations + 1;
                        }
                    }
                    index = mid;
                    if (in_array[mid + 1] == request)
                    {
                        while (in_array[index + 1] == request)
                        {
                            index = index + 1;
                            Console.WriteLine("Sorted in ascending order, we found {0} at index {1}.", request, index);
                            operations = operations + 1;
                        }
                    }
                    Console.WriteLine();
                    Console.WriteLine("This took {0} operations to complete.", operations);
                    break;
                }
                else if (request < in_array[mid])
                {
                    upper_bound = mid - 1;
                    closest_bigger.value = in_array[mid];
                    closest_bigger.index = mid;
                }
                else
                {
                    lower_bound = mid + 1;
                    closest_smaller.value = in_array[mid];
                    closest_smaller.index = mid;
                }
                operations = operations + 1;
            }
            if (!found)
            {
                double diff_sm = request - closest_smaller.value;
                if (diff_sm < 0)
                {
                    diff_sm *= -1;
                }
                double diff_bg = request - closest_bigger.value;
                if (diff_bg < 0)
                {
                    diff_bg *= -1;
                }
                Console.WriteLine("Search took {0} operations before coming to the following conclusion:", operations);
                if (diff_bg < diff_sm)
                {
                    Console.WriteLine("Couldn't find your {3}, but {0} at index {1} is only {2} away.", closest_bigger.value, closest_bigger.index, diff_bg, request);
                }
                else
                {
                    Console.WriteLine("Couldn't find your {3}, but {0} at index {1} is only {2} away.", closest_smaller.value, closest_smaller.index, diff_sm, request);
                }
                Console.WriteLine(Environment.NewLine + "No worries mate, thanks for looking. (press any key to continue)");
                Console.ReadLine();
                Console.Clear();
            }
            else
            {
                Console.WriteLine(Environment.NewLine + "Press any key to continue...");
                Console.ReadLine();
                Console.Clear();
            }
        }
    }

    class BST
    {
        public struct Tree_Info
        {
            public Tree binary_tree;
            public Node curr_root;
        }

        public static Tree_Info init_tree(double[] target_arr)
        {
            Node root = null;
            Tree binary_tree = new Tree();
            for (int i = 0; i < target_arr.Length; i = i +1)
            {
                root = binary_tree.insert(root, target_arr[i]);
            }
            Tree_Info tree_data = new Tree_Info();
            tree_data.binary_tree = binary_tree;
            tree_data.curr_root = root;
            return tree_data;
        }

        public class Node
        {
            public double value;
            public Node left;
            public Node right;
        }

        public class Tree
        {
            public Node insert(Node root, double element)
            {
                if (root == null)
                {
                    root = new Node();
                    root.value = element;
                }
                else if (element < root.value)
                {
                    root.left = insert(root.left, element);
                }
                else
                {
                    root.right = insert(root.right, element);
                }
                return root;
            }
        }
    }
}