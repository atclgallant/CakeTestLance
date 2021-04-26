
using System;
using System.Collections.Generic;
using System.Linq;


namespace ConsoleApp6
{
    class Program
    {
        /// <summary>        ///
        ///This console application calculates the optimal way to fill a bag with cakes given cake types with fixed weight and and value per cake
        ///The capacity of the bag is an int, and we can have a capacity of 0
        ///the weight of a cake can be 0 also 
        ///
        /// </summary>




        public class CakeType
        {
            public readonly int Weight;
            public readonly long Value;
         
            public CakeType(int weight, long value)
            {
                Weight = weight;
                Value = value;               
            }
        }

        public static void Main()
        {         
            CakeType[] cakeTypes = new[]
            {
                new CakeType(7, 160),
                new CakeType(3, 90),
                new CakeType(2, 15)
                

            };
                                                  
            int capacity = 20; //Set the default capacity. 

            //I'm allowing the user to input a different capacity if they want, so below is 
            //some UI stuff to allow the user to input a duffel capacity at runtime. Can be commented out to run without input            

            string val;
            Console.WriteLine("Welcome to your Cake Haul Calculator, o Prince of Thieves");
            Console.WriteLine("");
            Console.WriteLine("Please enter your duffel bag capacity (in kg's) as an integer.");
            Console.WriteLine("If you don't enter a valid value, I will assume you are carrying your usual " + capacity + "kg swag bag.");                      
            val = Console.ReadLine();
            
            //Validate that user has inputted a valid integer
            if (val.Length > 0 && IsAllDigits(val)== true) {
                capacity = Convert.ToInt32(val);
            }          

            Console.WriteLine("");
            Console.WriteLine("Your optimal strategy for filling a " + capacity + "kg bag is...");
            Console.WriteLine("");
            //End of user custom capacity section
            
            
            //Call the method to return the count and write to console
            long x = MaxDuffelBagValue(cakeTypes, capacity);

            Console.WriteLine("");
            Console.WriteLine("The maximum loot with a " + capacity +"kg bag is " + x + " shillings.");                   
            Console.ReadKey();
        }
        private static long MaxDuffelBagValue(CakeType[] cakeTypes, int capacity)           

        {
            //Set initial values for out running value and weight
            long runningDuffelBagValue = 0;
            int runningWeightValue = 0;
            int haulQty = 0; //A haul is a grouping of cakes of the same type (eg. 3 x 7kg cakes)
            int haulWeight = 0;

           

                List<CakeType> cakeTypeList = (from p in cakeTypes
                                               where p.Weight <= capacity && p.Weight > 0 //The weightless cake handled here, as is any cake heavier than the bag capacity
                                               orderby (p.Value / p.Weight) descending    //Ordered by the most valuable per kg.                                            
                                               select p).ToList();                        //The ordering is NB for what we do next...

            var mostValuableCake = cakeTypeList.FirstOrDefault(); //Take the first cake in the list -- MvC(most valuable cake...)             
                
                //See how many of the MvC's we can fit into the capacity.
                //If there's a remainder, try filling that with the next best cake that will fit in the space that remains
                               
                while (haulWeight <= capacity || cakeTypeList != null) 
                {
                    if (mostValuableCake != null) //There were cake types that meet the criteria
                    {                     
                        //Perform the necessary calculations to our MvC to create a haul
                        haulQty = (capacity - runningWeightValue) / mostValuableCake.Weight; //The max number of MvC I can add to the haul
                        haulWeight = haulWeight + (haulQty * mostValuableCake.Weight); //The total weight of the current MvC haul
                        runningDuffelBagValue = runningDuffelBagValue + (haulQty * mostValuableCake.Value); //A rnnuing sum of the total value                          
                        runningWeightValue = runningWeightValue + (haulQty * mostValuableCake.Weight); //A running sum of the weight         

                        //This just writes a haul by haul output as we go along. 
                        //Comment out to hide this feedback
                        Console.WriteLine("Take " + haulQty + " of the " + mostValuableCake.Weight + "kg cake (value: " + (haulQty * mostValuableCake.Value) + ")");
                        
                        //NB!!! Remove the cake types from the list, that cannot fit into the remaining capacity. 
                        cakeTypeList.RemoveAll(r => r.Weight > (capacity - haulWeight));

                        //The first item in this updated list is now the new MvC, ready for the next loop                        
                        mostValuableCake = cakeTypeList.FirstOrDefault();
                    }
                    else {
                        //There are no more possible cake types to try. Exit the loop
                        break;
                    }

                } 

                return runningDuffelBagValue;

         
        }
        
        static bool IsAllDigits(string s)
        {
            foreach (char c in s)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return true;
        }

       

    }

}



