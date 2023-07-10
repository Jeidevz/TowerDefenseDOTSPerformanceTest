using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[InternalBufferCapacity(4)]
public struct EntityBufferElement : IBufferElementData
{
    public Entity entity;
}
