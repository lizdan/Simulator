using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator.GUI
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading;
    using System.Windows.Threading;


    /// <summary>
    /// Manages the simulation of borrowing and returning shared assets within a family.
    /// It maintains a collection of assets and the status of each asset as it is borrowed and returned by family members.
    /// 
    /// Utilizes multi-threading to simulate activity for each family member.
    /// This class also ensures thread safety when modifying the collection of assets.
    /// 
    /// @Author Liza Danielsson.
    /// </summary>
    public class TaskManager
    {
        public ObservableCollection<Asset> Assets { get; } = new ObservableCollection<Asset>();
        private readonly List<string> memberNames = new List<string> { "Sven", "Greta", "Nisse", "Pelle", "Lotta" };
        private readonly List<Thread> memberThreads;
        private bool isRunning;
        private readonly Dispatcher dispatcher;

        public TaskManager(Dispatcher uiDispatcher)
        {
            dispatcher = uiDispatcher;
            memberThreads = new List<Thread>();
            isRunning = false;

            Assets.Add(new Asset("Screwdriver","Keep in original box!"));
            Assets.Add(new Asset("Ipad", "Only for studying"));
            Assets.Add(new Asset("Iphone charger", "Top hallway drawer when returning"));
            Assets.Add(new Asset("Massage gun", "Keep it fully charged"));
            Assets.Add(new Asset("Small car", "Only for shorter trips!"));
        }


        /// <summary>
        /// Adds a new asset to the collection with thread safety.
        /// </summary>
        public void AddAsset(string name, string description)
        {
            var asset = new Asset(name, description);
            lock (Assets) // Ensure thread safety when adding an asset
            {
                dispatcher.Invoke(() =>
                {
                    Assets.Add(asset);
                });
            }
        }


        /// <summary>
        /// Starts the simulation by creating and starting a thread for each family member.
        /// </summary>
        public void StartAllThreads()
        {
            isRunning = true;

            // Create a new thread for each member, add to list of threads and start.
            foreach (var memberName in memberNames)
            {
                var memberThread = new Thread(() => SimulateMemberActivity(memberName));
                memberThreads.Add(memberThread);
                memberThread.Start();
            }
        }



        /// <summary>
        /// Stops all threads, signaling the end of the simulation.
        /// </summary>
        public void StopAllThreads()
        {
            // Signal all threads to stop
            isRunning = false;

            // Wait for all threads to finish
            foreach (var thread in memberThreads)
            {
                thread.Join();
            }
        }



        /// <summary>
        /// Simulates the activity of a family member by randomly borrowing and returning assets.
        /// </summary>
        private void SimulateMemberActivity(string memberName)
        {
            Random random = new Random();

            while (isRunning)
            {
                // Randomly choose to borrow or return an item
                if (random.NextDouble() > 0.5)
                {
                    BorrowRandomAsset(memberName);
                }
                else
                {
                    ReturnRandomAsset(memberName);
                }

                // Simulate some delay between actions
                Thread.Sleep(random.Next(1000, 3000));
            }
        }



        /// <summary>
        /// Simulates a family member borrowing a random asset if available.
        /// </summary>
        private void BorrowRandomAsset(string memberName)
        {
            var random = new Random();

            lock (Assets)
            {
                dispatcher.Invoke(() =>
                {
                    if (Assets.Count > 0)
                    {
                        int index = random.Next(Assets.Count); 
                        var asset = Assets[index]; // Random asset to borrow

                        if (asset.Status == "Available") // Check if the asset is available to borrow
                        {
                            asset.Borrow(memberName);
                        }
                    }
                });
            }  
        }


        /// <summary>
        /// Simulates a family member returning a random asset they have borrowed.
        /// </summary>
        private void ReturnRandomAsset(string memberName)
        {
            var random = new Random();

            lock (Assets)
            {
                dispatcher.Invoke(() =>
                {
                    if (Assets.Count > 0)
                    {
                        int index = random.Next(Assets.Count);
                        var asset = Assets[index]; // Random asset to return

                        // Only return if the member currently has the asset borrowed.
                        if (asset.Status.Equals($"Borrowed by {memberName}"))
                        {
                            asset.Return();
                        }
                    }
                });
            }
            
        }
    }

}
