using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;
using Verse.AI;
using RimWorld;
using RimWorld.BaseGen;

namespace tribble
{

    public class LordJob_StayInPen : LordJob
    {
        private Faction faction;

        private IntVec3 baseCenter;

        public LordJob_StayInPen()
        {
        }

        public LordJob_StayInPen(Faction faction, IntVec3 baseCenter)
        {
            this.faction = faction;
            this.baseCenter = baseCenter;
        }

        public override StateGraph CreateGraph()
        {
            StateGraph stateGraph = new StateGraph();
            LordToil_StayInPen lordToil_StayInPen = new LordToil_StayInPen(this.baseCenter);
            stateGraph.StartingToil = lordToil_StayInPen;
            return stateGraph;
        }
    }
}
