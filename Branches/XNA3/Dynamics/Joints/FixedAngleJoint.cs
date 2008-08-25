using System;

namespace FarseerGames.FarseerPhysics.Dynamics.Joints
{
    public class FixedAngleJoint : Joint
    {
        private float _massFactor;
        private float _velocityBias;

        public FixedAngleJoint()
        {
            Breakpoint = float.MaxValue;
            MaxImpulse = float.MaxValue;
            BiasFactor = .2f;
        }

        public FixedAngleJoint(Body body)
        {
            Breakpoint = float.MaxValue;
            MaxImpulse = float.MaxValue;
            BiasFactor = .2f;
            Body = body;
        }

        public FixedAngleJoint(Body body, float targetAngle)
        {
            Breakpoint = float.MaxValue;
            MaxImpulse = float.MaxValue;
            BiasFactor = .2f;
            Body = body;
            TargetAngle = targetAngle;
        }

        public Body Body { get; set; }
        public float TargetAngle { get; set; }
        public float MaxImpulse { get; set; }

        public override void Validate()
        {
            if (Body.IsDisposed)
            {
                Dispose();
            }
        }

        public override void PreStep(float inverseDt)
        {
            base.PreStep(inverseDt);

            if (IsDisposed) return;

            Error = Body.TotalRotation - TargetAngle;

            _velocityBias = -BiasFactor*inverseDt*Error;
            _massFactor = (1 - Softness)/(Body.InverseMomentOfInertia);
        }

        public override void Update()
        {
            if (IsDisposed)
            {
                return;
            }
            float angularImpulse = (_velocityBias - Body.angularVelocity)*_massFactor;
            Body.angularVelocity += Body.InverseMomentOfInertia*Math.Sign(angularImpulse)*
                                    Math.Min(Math.Abs(angularImpulse), MaxImpulse);
        }
    }
}