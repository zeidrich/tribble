using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;
using Verse.AI;
using RimWorld;
using RimWorld.BaseGen;

namespace tribble
{
    public class LordToil_StayInPen : LordToil
    {
        public IntVec3 baseCenter;

        public override IntVec3 FlagLoc
        {
            get
            {
                return this.baseCenter;
            }
        }

        public LordToil_StayInPen(IntVec3 baseCenter)
        {
            this.baseCenter = baseCenter;
        }

        public override void UpdateAllDuties()
        {
            for (int i = 0; i < this.lord.ownedPawns.Count; i++)
            {
                DutyDef dd;
                dd = DefDatabase<DutyDef>.GetNamed("StayInPen", true);
                this.lord.ownedPawns[i].mindState.duty = new PawnDuty(dd, this.baseCenter,-1f);
            }
        }
    }
}
