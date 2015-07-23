// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Rotates a RigidBody. It's more efficient then accessing the transform of the gameObject.")]
	public class PhysicsMoveRotation : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Rigidbody))]
		[Tooltip("The rigid body to rotate.")]
		public FsmOwnerDefault gameObject;

		[Tooltip("A rotation quaternion.")]
		[UIHint(UIHint.Variable)]
		public FsmQuaternion rotation;
		
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
		
		private Rigidbody rigidBody;
		
		public override void Reset()
		{
			gameObject = null;
			rotation = null;
			everyFrame = true;
		}

        public override void Awake()
        {
            Fsm.HandleFixedUpdate = true;
        }
		
		public override void OnEnter()
		{
			
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				Finish();
				return;
			}
			
			rigidBody = go.GetComponent<Rigidbody>();
			if (rigidBody == null)
			{
				Finish();
				return;
			}
				
		}

        public override void OnFixedUpdate()
        {
            DoRotate();
            

            if (!everyFrame)
            {
                Finish();
            }
        }

		void DoRotate()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				return;
			}

			rigidBody.MoveRotation(rotation.Value);

		}

	}
}