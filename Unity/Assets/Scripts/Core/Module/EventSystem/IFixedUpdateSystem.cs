﻿using System;

namespace ET
{
	public interface IFixedUpdate
	{
	}
	
	public interface IFixedUpdateSystem: ISystemType
	{
		void Run(Entity o);
	}

	[ObjectSystem]
	public abstract class FixedUpdateSystem<T> : IFixedUpdateSystem where T: Entity, IFixedUpdate
	{
		void IFixedUpdateSystem.Run(Entity o)
		{
			this.FixedUpdate((T)o);
		}

		Type ISystemType.Type()
		{
			return typeof(T);
		}

		Type ISystemType.SystemType()
		{
			return typeof(IFixedUpdateSystem);
		}

		InstanceQueueIndex ISystemType.GetInstanceQueueIndex()
		{
			return InstanceQueueIndex.FixedUpdate;
		}

		protected abstract void FixedUpdate(T self);
	}
}
