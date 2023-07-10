using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Unity.Jobs;
using Unity.Burst;

namespace Testing
{
    public class TestJobExample1 : MonoBehaviour
    {
        public enum RunMode
        {
            MainThread,
            Parallel,
            Concurrent
        }

        public RunMode mode = RunMode.MainThread;
        public KeyCode runKey;
        public int amount = 10000;
        public int valueMax = 10;
        public int valueMin = 1;

        public struct Data
        {
            public int firstValue;
            public int secondValue;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(runKey))
            {
                float timeNow = Time.realtimeSinceStartup;

                NativeArray<Data> datas = new NativeArray<Data>(amount, Allocator.TempJob);
                NativeArray<int> results = new NativeArray<int>(amount, Allocator.TempJob);

                if (mode == RunMode.MainThread)
                    RunMainThread(ref datas, ref results, amount, valueMin, valueMax);
                else if (mode == RunMode.Concurrent)
                {
                    JobHandle concurrentHandle = RunConcurrent(ref datas, ref results, amount, valueMin, valueMax);
                    concurrentHandle.Complete();
                }
                else if (mode == RunMode.Parallel)
                {
                    JobHandle parallelHandle = RunParallel(ref datas, ref results, amount, valueMin, valueMax);
                    parallelHandle.Complete();
                }

                float time = Time.realtimeSinceStartup - timeNow;

                //for (int i = 0; i < amount; i++)
                //{
                //    Debug.Log(datas[i].firstValue + " - " + datas[i].secondValue + " = " + results[i]);
                //}

                Debug.Log("Time took: " + time);

                datas.Dispose();
                results.Dispose();
            }
        }

        private JobHandle RunParallel(ref NativeArray<Data> datas, ref NativeArray<int> results, int amount, int min, int max)
        {
            int batch = 32;

            ParallelJob fillJob = new ParallelJob
            {
                datas = datas,
                maxRange = max,
                minRange = min
            };

            JobHandle fillJobHandle = fillJob.Schedule(amount, batch);

            SubtractParallelJob substractParallelJob = new SubtractParallelJob
            {
                datas = datas,
                result = results
            };
            
            JobHandle substactParallelJobHandle = substractParallelJob.Schedule(amount, batch, fillJobHandle);
            return substactParallelJobHandle;


        }

        private JobHandle RunConcurrent(ref NativeArray<Data> datas, ref NativeArray<int> results, int amount, int min, int max)
        {
            ConcurrentJob fillJob = new ConcurrentJob
            {
                datas = datas,
                maxRange = max,
                minRange = min
            };

            JobHandle fillJobHandle = fillJob.Schedule();

            SubstractConcurrentJob substractConcurrentJob = new SubstractConcurrentJob
            {
                datas = datas,
                result = results
            };

            JobHandle substactConcurrentJobHandle = substractConcurrentJob.Schedule(amount, fillJobHandle);
            return substactConcurrentJobHandle;
        }

        private void RunMainThread(ref NativeArray<Data> datas, ref NativeArray<int> results, int amount, int min, int max)
        {
            for (int i = 0; i < amount; i++)
            {
                int first = Random.Range(min, max);
                int second = Random.Range(min, max);
                datas[i] = new Data
                {
                    firstValue = first,
                    secondValue = second
                };

                int y = 0;
                while (y < 100000)
                {
                    y++;
                }

                results[i] = datas[i].firstValue - datas[i].secondValue;

                int z = 0;
                while (z < 100000)
                {
                    z++;
                }


            }
        }

        private struct ConcurrentJob : IJob
        {
            public NativeArray<Data> datas;
            public int maxRange;
            public int minRange;
            public void Execute()
            {
                for(int i = 0; i < datas.Length; i++)
                {
                    int first = Random.Range(minRange, maxRange);
                    int second = Random.Range(minRange, maxRange);
                    datas[i] = new Data
                    {
                        firstValue = first,
                        secondValue = second
                    };

                    int y = 0;
                    while (y < 100000)
                    {
                        y++;
                    }
                }
            }
        }

        private struct SubstractConcurrentJob : IJobFor
        {
            public NativeArray<Data> datas;
            public NativeArray<int> result;
            public void Execute(int index)
            {
                result[index] = datas[index].firstValue - datas[index].secondValue;

                int i = 0;
                while (i < 100000)
                {
                    i++;
                }
            }
        }

        [BurstCompile]
        private struct ParallelJob : IJobParallelFor
        {
            public NativeArray<Data> datas;
            public int maxRange;
            public int minRange;
            public void Execute(int index)
            {
                int first = Random.Range(minRange, maxRange);
                int second = Random.Range(minRange, maxRange);
                datas[index] = new Data
                {
                    firstValue = first,
                    secondValue = second
                };

                int i = 0;
                while (i < 100000)
                {
                    i++;
                }

            }
        }

        private struct SubtractParallelJob : IJobParallelFor
        {
            public NativeArray<Data> datas;
            public NativeArray<int> result;
            public void Execute(int index)
            {
                result[index] = datas[index].firstValue - datas[index].secondValue;
                int i = 0;
                while (i < 100000)
                {
                    i++;
                }
            }
        }
    }
}