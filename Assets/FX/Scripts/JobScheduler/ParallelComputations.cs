using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
class ParallelComputations
{

    public static DistancesJob CreateParallelDistancesJob(List<Vector3> fromPositions, Vector3 destinationPosition)
    {
        var length = fromPositions.Count;
        NativeArray<Vector3> from = new NativeArray<Vector3>(length, Allocator.TempJob);
        NativeArray<Vector3> to = new NativeArray<Vector3>(length, Allocator.TempJob);
        NativeArray<float> result = new NativeArray<float>(length, Allocator.TempJob);

        for (int i = 0; i < length; i++)
        {
            from[i] = fromPositions[i];
            to[i] = destinationPosition;
        }
        return new DistancesJob() { From = from, To = to, Result = result };
    }

    public struct DistancesJob : IJobParallelFor
    {
        public NativeArray<Vector3> From;
        public NativeArray<Vector3> To;
        public NativeArray<float> Result;

        public void Execute(int i)
        {
            Result[i] = Vector3.Distance(From[i], To[i]);
        }
    }

    public static void FreeDistancesJobMemory(DistancesJob job)
    {
        job.From.Dispose();
        job.To.Dispose();
        job.Result.Dispose();
    }
}
