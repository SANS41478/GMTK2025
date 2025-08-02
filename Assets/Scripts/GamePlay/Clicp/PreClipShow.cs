using System;
using System.Collections.Generic;
using GamePlay.Entity;
using Lifecycels;
using Space.EventFramework;
using Space.GlobalInterface.Lifecycle;
using Space.LifeControllerFramework.PipelineLifeController;
using UnityEngine;
using Utility;
namespace GamePlay
{
    //TODO:
    public class PreClipShow : MonoBehaviour  , IInputRefresh
    {
        /// <summary>
        /// 模拟生命周期 
        /// </summary>
        private ILifecycleManager selfManager=new LifecyclePipelineManager();
        private LifeManager _lifeManager;
        private ShadowCalculate calculateShadow;
        private EntityInfo preEntityInfo ;
        private void Update()
        {
            preEntityInfo.Position = WorldCellTool.WorldToCell(this.transform.position);
        }
        public void ShowPreClip()
        {
            _lifeManager=  new LifeManager(selfManager);
        }
        public void Refresh()
        {
            ShowPreClip();
        }
    }
}