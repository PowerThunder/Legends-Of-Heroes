﻿using System;

namespace ET.Server
{
    [ObjectSystem]
    public class LocationProxyComponentAwakeSystem : AwakeSystem<LocationProxyComponent>
    {
        protected override void Awake(LocationProxyComponent self)
        {
            LocationProxyComponent.Instance = self;
        }
    }

    [ObjectSystem]
    public class LocationProxyComponentDestroySystem : DestroySystem<LocationProxyComponent>
    {
        protected override void Destroy(LocationProxyComponent self)
        {
            LocationProxyComponent.Instance = null;
        }
    }

    public static class LocationProxyComponentSystem
    {
        private static long GetLocationSceneId(long key)
        {
            return StartSceneConfigCategory.Instance.LocationConfig.InstanceId;
        }

        public static async ETTask Add(this LocationProxyComponent self, int type, long key, long instanceId)
        {
            Log.Info($"location proxy add {key}, {instanceId} {TimeHelper.ServerNow()}");
            await ActorMessageSenderComponent.Instance.Call(GetLocationSceneId(key),
                new ObjectAddRequest() { Type = type, Key = key, InstanceId = instanceId });
        }

        public static async ETTask Lock(this LocationProxyComponent self, int type, long key, long instanceId, int time = 60000)
        {
            Log.Info($"location proxy lock {key}, {instanceId} {TimeHelper.ServerNow()}");
            await ActorMessageSenderComponent.Instance.Call(GetLocationSceneId(key),
                new ObjectLockRequest() { Type = type, Key = key, InstanceId = instanceId, Time = time });
        }

        public static async ETTask UnLock(this LocationProxyComponent self, int type, long key, long oldInstanceId, long instanceId)
        {
            Log.Info($"location proxy unlock {key}, {instanceId} {TimeHelper.ServerNow()}");
            await ActorMessageSenderComponent.Instance.Call(GetLocationSceneId(key),
                new ObjectUnLockRequest() { Type = type, Key = key, OldInstanceId = oldInstanceId, InstanceId = instanceId });
        }

        public static async ETTask Remove(this LocationProxyComponent self, int type, long key)
        {
            Log.Info($"location proxy add {key}, {TimeHelper.ServerNow()}");
            await ActorMessageSenderComponent.Instance.Call(GetLocationSceneId(key),
                new ObjectRemoveRequest() { Type = type, Key = key });
        }

        public static async ETTask<long> Get(this LocationProxyComponent self, int type, long key)
        {
            if (key == 0)
            {
                throw new Exception($"get location key 0");
            }

            // location server配置到共享区，一个大战区可以配置N多个location server,这里暂时为1
            ObjectGetResponse response =
                    (ObjectGetResponse)await ActorMessageSenderComponent.Instance.Call(GetLocationSceneId(key),
                        new ObjectGetRequest() { Type = type, Key = key });
            return response.InstanceId;
        }

        public static async ETTask AddLocation(this Entity self, int type)
        {
            await LocationProxyComponent.Instance.Add(type, self.Id, self.InstanceId);
        }

        public static async ETTask RemoveLocation(this Entity self, int type)
        {
            await LocationProxyComponent.Instance.Remove(type, self.Id);
        }
    }
}