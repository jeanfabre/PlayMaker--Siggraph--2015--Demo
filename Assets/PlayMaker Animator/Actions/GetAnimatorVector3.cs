#if false
// `UnityEngine.Animator.GetVector(int)' is obsolete: `GetVector is deprecated.'


// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Animator")]
	[Tooltip("Gets the value of a vector3 parameter")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W1059")]
	public class GetAnimatorVector3 : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Animator))]
		[Tooltip("The target. An Animator component and a PlayMakerAnimatorProxy component are required")]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("The animator parameter")]
		public FsmString parameter;
		
		[Tooltip("Repeat every frame. Useful when value is subject to change over time.")]
		public bool everyFrame;
		
		[ActionSection("Results")]
		
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The Vector3 value of the animator parameter")]
		public FsmVector3 result;
		
		private PlayMakerAnimatorMoveProxy _animatorProxy;
		
		private Animator _animator;
		
		private int _paramID;
		
		public override void Reset()
		{
			gameObject = null;
			parameter = null;
			result = null;
			everyFrame = false;
		}
		
		
		// Code that runs on entering the state.
		public override void OnEnter()
		{
			// get the animator component
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			
			if (go==null)
			{
				Finish();
				return;
			}
			
			_animator = go.GetComponent<Animator>();
			
			if (_animator==null)
			{
				Finish();
				return;
			}
			
			_animatorProxy = go.GetComponent<PlayMakerAnimatorMoveProxy>();
			if (_animatorProxy!=null)
			{
				_animatorProxy.OnAnimatorMoveEvent += OnAnimatorMoveEvent;
			}
			
			// get hash from the param for efficiency:
			_paramID = Animator.StringToHash(parameter.Value);
			
			GetParameter();
			
			if (!everyFrame) 
			{
				Finish();
			}
		}
	
		public void OnAnimatorMoveEvent()
		{
			if (_animatorProxy!=null)
			{
				GetParameter();
			}
		}	
		
		public override void OnUpdate() 
		{
			if (_animatorProxy==null)
			{
				GetParameter();
			}
		}
		
		void GetParameter()
		{		
			if (_animator!=null)
			{
				result.Value = _animator.GetVector(_paramID);
			}
		}
		
		public override void OnExit()
		{
			if (_animatorProxy!=null)
			{
				_animatorProxy.OnAnimatorMoveEvent -= OnAnimatorMoveEvent;
			}
		}
	}
}
#endif