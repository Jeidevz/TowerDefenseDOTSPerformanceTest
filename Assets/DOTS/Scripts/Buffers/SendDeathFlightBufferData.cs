using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[InternalBufferCapacity(300)]
public struct SendDeathFlightBufferData : IBufferElementData
{
    public Entity entity;
    public float3 point;
    public float3 normal;
    public float force;
    public float deathTimer;
}
