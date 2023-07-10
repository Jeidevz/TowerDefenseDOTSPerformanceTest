using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[InternalBufferCapacity(100)]
public struct SpawnEntityBuffer : IBufferElementData
{
    public Entity entity;
    public float3 translation;
    public float3 rotation;
}
