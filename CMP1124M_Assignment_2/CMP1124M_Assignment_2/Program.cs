using System;
using System.Collections.Generic;

namespace CMP1124M_Assignment_2
{
    class Program
    {
        public static class Datasets
        {
            public static double[] High_256 = resources(Properties.Resources.High_256);
            public static double[] High_4096 = resources(Properties.Resources.High_4096);
            public static double[] High_2048 = resources(Properties.Resources.High_2048);
            public static double[] Low_256 = resources(Properties.Resources.Low_256);
            public static double[] Low_2048 = resources(Properties.Resources.Low_2048);
            public static double[] Low_4096 = resources(Properties.Resources.Low_4096);
            public static double[] Mean_2048 = resources(Properties.Resources.Mean_2048);
            public static double[] Mean_256 = resources(Properties.Resources.Mean_256);
            public static double[] Mean_4096 = resources(Properties.Resources.Mean_4096);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Please choose one of the following:");
            Console.WriteLine("3 - High 256");
            Console.WriteLine("2 - Mean 256");
            Console.WriteLine("1 - Low 256");
            Console.Write("Data set: ");
            int user_input = input_validation(Console.ReadLine(), 3);
            Console.Clear();
            Arrays_Sorted Arrays_Sorted = run_sort(user_input, false);
            Console.WriteLine();
            Console.Write("Press any button to continue");
            Console.ReadLine();
            Console.Clear();
            Console.Write("What value do you want to search for?");
            double user_selection = entry_validation(Console.ReadLine());
            binary_search_alg(user_selection, Arrays_Sorted.ascending_sorted);
            linear_search(user_selection, Arrays_Sorted.ascending_sorted);
            Console.WriteLine("Now for the larger data sets:");
            Console.WriteLine("1 - Low 2048");
            Console.WriteLine("2 - Low 4096");
            Console.WriteLine("3 - Mean 2048");
            Console.WriteLine("4 - Mean 4096");
            Console.WriteLine("5 - High 2048");
            Console.WriteLine("6 - High 4096");
            Console.Write("Data set: ");
            user_input = input_validation(Console.ReadLine(), 6);
            Console.Clear();
            run_sort(user_input, true);
            Console.WriteLine();
            Console.Write("Press any button to continue");
            Console.ReadLine();
            Console.Clear();
            Console.Write("Enter a value to search for");
            user_selection = entry_validation(Console.ReadLine());
            Arrays_Sorted = run_sort(user_input, true);
            binary_search_alg(user_selection, Arrays_Sorted.ascending_sorted);
            linear_search(user_selection, Arrays_Sorted.ascending_sorted);
            for (int data_set = 0; data_set < 3; data_set = data_set + 1)
            {
                Arrays_Sorted = array_sort(array_merging(data_set));
                searching(Arrays_Sorted.ascending_sorted);
            }
            Console.Write("Would you like to go again?(y/n)");
            if (Console.ReadLine() == "y")
            {
                Console.Clear();
                Main(null);
            }
            Environment.Exit(0);
        }

        public static double[] resources(string resource)
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
            int iterations = array_size_finder(target);
            Tree_Data bst_tree = init_tree(target);
            Tree binary_tree = bst_tree.binary_tree;
            sorted_ascending_array.Clear();
            sorted_descending_array.Clear();
            Console.WriteLine();
            bubble_sort_ascending(target);
            Console.WriteLine("These are the values at every {0}th value in ascending order:", iterations);
            Binary_st_ascending(bst_tree.root_current);
            double[] ascending_sorted = sorted_ascending_array.ToArray();
            display_values(ascending_sorted, iterations, large_data_set);
            return_vals.ascending_sorted = ascending_sorted;
            Console.WriteLine();
            bubble_sort_descending(target);
            Console.WriteLine("These are the values at every {0}th value in descending order:", iterations);
            Binary_st_descending(bst_tree.root_current);
            double[] descending_sorted = sorted_descending_array.ToArray();
            display_values(descending_sorted, iterations, large_data_set);
            return_vals.descending_sorted = descending_sorted;
            return return_vals;
        }

        public static Arrays_Sorted array_sort(double[] target)
        {
            Arrays_Sorted return_vals = new Arrays_Sorted();
            int iterations = 10;
            Tree_Data direct_bst_tree = init_tree(target);
            Tree binary_tree = direct_bst_tree.binary_tree;
            sorted_ascending_array.Clear();
            sorted_descending_array.Clear();
            Console.WriteLine();
            Console.WriteLine("These are the values at every {0}th value in ascending order:", iterations);
            Binary_st_ascending(direct_bst_tree.root_current);
            double[] ascending_sorted = sorted_ascending_array.ToArray();
            display_values(ascending_sorted, iterations, false);
            return_vals.ascending_sorted = ascending_sorted;
            Console.WriteLine();
            Console.WriteLine("These are the values at every {0}th value in descending order:", iterations);
            Binary_st_descending(direct_bst_tree.root_current);
            double[] descending_sorted = sorted_descending_array.ToArray();
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

        public static double[] chosen_array(int selection, bool large)
        {
            double[] chosen_data = null;
            if (large == false)
            {
                switch (selection)
                {
                    case 1:
                        chosen_data = Datasets.Low_256;
                        break;
                    case 2:
                        chosen_data = Datasets.Mean_256;
                        break;
                    case 3:
                        chosen_data = Datasets.High_256;
                        break;
                }
            }
            else
            {
                switch (selection)
                {
                    case 1:
                        chosen_data = Datasets.Low_2048;
                        break;
                    case 2:
                        chosen_data = Datasets.Low_4096;
                        break;
                    case 3:
                        chosen_data = Datasets.Mean_2048;
                        break;
                    case 4:
                        chosen_data = Datasets.Mean_4096;
                        break;
                    case 5:
                        chosen_data = Datasets.High_2048;
                        break;
                    case 6:
                        chosen_data = Datasets.High_4096;
                        break;
                }
            }
            return chosen_data;
        }

        public static double[] array_merging(int array_size_finder)
        {
            double[] array_upper = null;
            double[] array_lower = null;
            int size = 0;
            switch (array_size_finder)
            {
                case 0:
                    array_lower = Datasets.Low_256;
                    array_upper = Datasets.High_256;
                    size = 256;
                    break;
                case 1:
                    array_lower = Datasets.Low_2048;
                    array_upper = Datasets.High_2048;
                    size = 2048;
                    break;
                case 2:
                    array_lower = Datasets.Low_4096;
                    array_upper = Datasets.High_4096;
                    size = 4096;
                    break;
            }
            Console.WriteLine("Output of the merging of the Low and High {0} arrays:", size);
            double[] merged_array = new double[array_lower.Length + array_upper.Length];
            Array.Copy(array_lower, merged_array, array_lower.Length);
            Array.Copy(array_upper, 0, merged_array, array_lower.Length, array_upper.Length);
            return merged_array;
        }

        public static void searching(double[] merged_array)
        {
            Console.WriteLine();
            Console.Write("Press any button to continue");
            Console.ReadLine();
            Console.Clear();
            Console.Write("enter a value you want to search for: ");
            double user_selection = entry_validation(Console.ReadLine());
            binary_search_alg(user_selection, merged_array);
            linear_search(user_selection, merged_array);
        }

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
        public static void Binary_st_ascending(Node root)
        {
            if (root == null)
            {
                return;
            }
            Binary_st_ascending(root.left);
            sorted_ascending_array.Add(root.value);
            Binary_st_ascending(root.right);
        }

        public static void Binary_st_descending(Node root)
        {
            if (root == null)
            {
                return;
            }
            Binary_st_descending(root.right);
            sorted_descending_array.Add(root.value);
            Binary_st_descending(root.left);
        }

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
            Console.WriteLine("These are the results from the ascending order search:");
            if (found)
            {
                int[] indexes_arr = indexes.ToArray();
                foreach (int index in indexes_arr)
                {
                    Console.WriteLine("Your number at was found at index {0}", index);
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
                Console.WriteLine("We couldn't find your number, the closest we could find is {0} at {1} index number.", closest_value, closest_index);
            }
        }

        struct Num_Substitution
        {
            public double value;
            public int index;
        }

        public static void bubble_sort_ascending(double[] input_arr)
        {
            int ops = 0;
            int increments = array_size_finder(input_arr);
            double temp = 0;
            for (int write = 0; write < input_arr.Length; write++)
            {
                for (int sort = 0; sort < input_arr.Length - 1; sort++)
                {
                    if (input_arr[sort] > input_arr[sort + 1])
                    {
                        temp = input_arr[sort + 1];
                        input_arr[sort + 1] = input_arr[sort];
                        input_arr[sort] = temp;
                        ops = ops + 1;
                    }
                }
            }
            Console.WriteLine("The total number of operations for ascending bubble sort was: {0}", ops);
        }

        public static void bubble_sort_descending(double[] input_arr)
        {
            int ops = 0;
            int increments = array_size_finder(input_arr);
            double temp = 0;
            for (int write = 0; write < input_arr.Length; write++)
            {
                for (int sort = 1; sort < input_arr.Length; sort++)
                {
                    if (input_arr[sort] > input_arr[sort - 1])
                    {
                        temp = input_arr[sort - 1];
                        input_arr[sort - 1] = input_arr[sort];
                        input_arr[sort] = temp;
                        ops = ops + 1;
                    }
                }
            }
            Console.WriteLine("The total number of operations for the descending bubble sort was: {0}", ops);
        }

        public static void binary_search_alg(double request, double[] in_array)
        {
            Console.Clear();
            bool found = false;
            int lower_bound = 0;
            int upper_bound = in_array.Length;
            Num_Substitution closest_smaller = new Num_Substitution();
            Num_Substitution closest_bigger = new Num_Substitution();
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
                    Console.WriteLine("Your number {3} couldn't be found, but we found {0} at index number {1}.", closest_bigger.value, closest_bigger.index, diff_bg, request);
                }
                else
                {
                    Console.WriteLine("Your number {3} couldn't be found, but we found {0} at index number {1}.", closest_smaller.value, closest_smaller.index, diff_sm, request);
                }
            }
        }

        public static void linear_search(double target, double[] given_array)
        {
            int array_size = given_array.Length;
            int ops = 0;
            bool found = false;
            double closest_num = 0;
            int closest_index = 0;
            double current_diff = 99999;
            for (int iteration = 0; iteration < array_size - 1; iteration++)
            {
                if (given_array[iteration] == target)
                {
                    found = true;
                    Console.WriteLine("Number found at index {0}", iteration);
                }
                else
                {
                    if (!found)
                    {
                        ops = ops + 1;
                        double difference = target - given_array[iteration];
                        if (difference < 0)
                        {
                            difference = difference * -1;
                        }
                        if (difference < current_diff)
                        {
                            closest_index = iteration;
                            closest_num = given_array[iteration];
                            current_diff = difference;
                        }
                    }
                }
            }
            Console.WriteLine();
            Console.WriteLine("Linear search:");
            if (!found)
            {
                Console.WriteLine("Could not find your numbers. Instead found {0} at index {1} which is the closest.", closest_num, closest_index);
            }
            Console.WriteLine("Linear search for {0} took {1} iterations before it was complete/found the first instance.", target, ops);
            Console.WriteLine();
            Console.ReadLine();
            Console.Clear();
        }

        public struct Tree_Data
        {
            public Tree binary_tree;
            public Node root_current;
        }

        public static Tree_Data init_tree(double[] targeted_array)
        {
            Node root = null;
            Tree binary_tree = new Tree();
            for (int i = 0; i < targeted_array.Length; i = i + 1)
            {
                root = binary_tree.insertion(root, targeted_array[i]);
            }
            Tree_Data tree_data = new Tree_Data();
            tree_data.binary_tree = binary_tree;
            tree_data.root_current = root;
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
            public Node insertion(Node root, double element)
            {
                if (root == null)
                {
                    root = new Node();
                    root.value = element;
                }
                else if (element < root.value)
                {
                    root.left = insertion(root.left, element);
                }
                else
                {
                    root.right = insertion(root.right, element);
                }
                return root;
            }
        }
    }
}