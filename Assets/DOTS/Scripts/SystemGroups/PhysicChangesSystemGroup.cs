using System;
using Unity.Entities;

namespace TowerDefenseDOTS
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup)),UpdateBefore(typeof(Unity.Physics.Systems.BuildPhysicsWorld))]
    public class PhysicChangesSystemGroup : ComponentSystemGroup
    {

    }
}