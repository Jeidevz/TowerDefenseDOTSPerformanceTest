using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Unity.Jobs;

namespace Testing
{
    public class TestJobExample : MonoBehaviour
    {
        public enum RunMode
        {
            MainThread,
            Parallel,
            Concurrent
        }

        public RunMode mode = RunMode.MainThread;
        public KeyCode runKey;
        public int amount = 10;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(runKey))
            {
                float timeNow = Time.realtimeSinceStartup;


                if (mode == RunMode.MainThread)
                    RunMainThread(amount);
                else if (mode == RunMode.Concurrent)
                {
                    JobHandle concurrentHandle = RunConcurrent(amount);
                    concurrentHandle.Complete();
                }
                else if (mode == RunMode.Parallel)
                {
                    JobHandle parallelHandle = RunParallel(amount);
                    parallelHandle.Complete();
                }

                float time = Time.realtimeSinceStartup - timeNow;

                Debug.Log("Time took: " + time);
            }
        }

        private JobHandle RunParallel(int amount)
        {
            int batch = 32;
            ParallelJob parallelJob = new ParallelJob{};
            return parallelJob.Schedule(amount, batch);
        }

        private JobHandle RunConcurrent(int amount)
        {
            ConcurrentJob concurrentJob = new ConcurrentJob{};
            JobHandle handle = new JobHandle();
            return concurrentJob.Schedule(amount, handle);
        }

        private void RunMainThread(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                for(int z = 0; z < 10000; z++)
                {

                }
            }
        }

        private struct ConcurrentJob : IJobFor
        {
            public void Execute(int index)
            {
                for (int z = 0; z < 10000; z++)
                {

                }
            }
        }

        private struct ParallelJob : IJobParallelFor
        {
            public void Execute(int index)
            {
                for (int z = 0; z < 10000; z++)
                {

                }
            }
        }

        private struct JobParallel : IJobParallelFor
        {
            public NativeArray<int> numbers;
            public int multiplier;
            public void Execute(int index)
            {
                int number = numbers[index] * multiplier;
                numbers[index] = number;
            }
        }

        private struct JobConcurrent : IJob
        {
            public NativeArray<int> numbers;
            public int multiplier;

            public void Execute()
            {
                for(int i = 0; i < numbers.Length; i++)
                {
                    int number = numbers[i] * multiplier;
                    numbers[i] = number;
                }
            }
        }
        private void TestJobFunction()
        {
            int arraySize = 1000;
            NativeArray<int> listNumbers = new NativeArray<int>(arraySize, 
                Allocator.TempJob);

            //Fill list with data.
            for(int i = 0; i < arraySize; i++)
            {
                listNumbers[i] = i;
            }

            JobConcurrent jobConcurrent = new JobConcurrent
            {
                numbers = listNumbers,
                multiplier = 2
            };

            JobHandle jobConcurrentHandle = jobConcurrent.Schedule();

            int minAmountEaBatch = 32;
            JobParallel jobParallel = new JobParallel { 
                numbers = listNumbers,
                multiplier = 3
            };
            JobHandle jobParallelHandle = jobParallel.Schedule(arraySize, 
                minAmountEaBatch, jobConcurrentHandle);
            jobParallelHandle.Complete();

            listNumbers.Dispose();
        }
    }


}