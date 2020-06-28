// Decompiled with JetBrains decompiler
// Type: SmartBear.Collaborator.Api.ProgressMonitor
// Assembly: Collaborator.Api, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9c3e7b13c78c6163
// MVID: 82D41E82-1BF6-4105-8B80-342693CB64ED
// Assembly location: C:\Users\fyu\AppData\Local\Microsoft\VisualStudio\16.0_be238a39\Extensions\gzzm4hy2.wuy\Collaborator.Api.dll

using System;
using System.Threading;

namespace CollabAPI
{
  public class ProgressMonitor : IProgressMonitor
  {
    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    private bool _isHidden = true;
    private string _titleText;
    private string _waitMessage;
    private string _progressText;
    private string _statusBarText;
    private int _totalStepsCount;
    private int _complitedStepsCount;
    private bool _disableCancel;
    private bool _isCancelable;
    private bool _isCanceled;

    public ProgressMonitor(string titleText)
    {
    }

    public ProgressMonitor(string titleText, bool isCancelable)
      : this(titleText)
    {
      this._isCancelable = isCancelable;
    }

    public ProgressMonitor(
      string titleText,
      bool isCancelable,
      int totalStepsCount,
      int complitedStepsCount)
      : this(titleText, isCancelable)
    {
      this._totalStepsCount = totalStepsCount;
      this._complitedStepsCount = complitedStepsCount;
    }

    private void DoUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      //this._waitDialog.UpdateProgress(this._waitMessage, this._progressText, this._statusBarText, this._totalStepsCount, this._complitedStepsCount, this._disableCancel, out bool _);
    }

    private void Update()
    {
        this.DoUpdate();
    }


    public string WaitMessage
    {
      get
      {
        return this._waitMessage;
      }
      set
      {
        this._waitMessage = value;
        this.Update();
      }
    }

    public string ProgressText
    {
      get
      {
        return this._progressText;
      }
      set
      {
        this._progressText = value;
        this.Update();
      }
    }

    public string StatusBarText
    {
      get
      {
        return this._statusBarText;
      }
      set
      {
        this._statusBarText = value;
        this.Update();
      }
    }

    public bool HasCanceled
    {
      get
      {
                return false;
      }
    }

    public bool IsHidden
    {
      get
      {
        return this._isHidden;
      }
    }

    private void DoHide()
    {
    }

    public void Hide()
    {
    }

    private void DoShow()
    {
    }

    public void Show()
    {
    }

    public int TotalStepsCount
    {
      get
      {
        return this._totalStepsCount;
      }
      set
      {
        this._totalStepsCount = value;
        this.Update();
      }
    }

    public int CompletedStepsCount
    {
      get
      {
        return this._complitedStepsCount;
      }
      set
      {
        this._complitedStepsCount = value;
        this.Update();
      }
    }

    public CancellationTokenSource CancellationTokenSource
    {
      get
      {
        return this._cancellationTokenSource;
      }
    }
  }
}
