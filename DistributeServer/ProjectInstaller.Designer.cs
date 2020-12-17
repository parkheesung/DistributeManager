﻿
namespace DistributeServer
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.DistributeManagerServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.DistributeManagerServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // DistributeManagerServiceProcessInstaller
            // 
            this.DistributeManagerServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.DistributeManagerServiceProcessInstaller.Password = null;
            this.DistributeManagerServiceProcessInstaller.Username = null;
            // 
            // DistributeManagerServiceInstaller
            // 
            this.DistributeManagerServiceInstaller.DisplayName = "배포관리매니저";
            this.DistributeManagerServiceInstaller.ServiceName = "DistributeManager";
            this.DistributeManagerServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.DistributeManagerServiceProcessInstaller,
            this.DistributeManagerServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller DistributeManagerServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller DistributeManagerServiceInstaller;
    }
}