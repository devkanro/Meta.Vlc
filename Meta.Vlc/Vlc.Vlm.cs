// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: Vlc.Vlm.cs
// Version: 20160214

using System;
using System.Runtime.InteropServices;
using Meta.Vlc.Interop;
using Meta.Vlc.Interop.Core.Events;
using Meta.Vlc.Interop.VLM;

namespace Meta.Vlc
{
    public partial class Vlc : IVlcObjectWithEvent
    {
        private static LibVlcFunction<ReleaseVlmInstance> _releaseVlmInstanceFunction;
        private static LibVlcFunction<NewBroadCastInput> _newBroadCastInputFunction;
        private static LibVlcFunction<NewVodInput> _newVodInputFunction;
        private static LibVlcFunction<DelBoroadcastOrOvd> _delBoroadcastOrOvdFunction;
        private static LibVlcFunction<MediaSwitch> _mediaSwitchFunction;
        private static LibVlcFunction<SetMediaOutput> _setMediaOutputFunction;
        private static LibVlcFunction<SetMediaInput> _setMediaInputFunction;
        private static LibVlcFunction<AddMediaInput> _addMediaInputFunction;
        private static LibVlcFunction<SetMediaLoop> _setMediaLoopFunction;
        private static LibVlcFunction<SetVodMuxer> _setVodMuxerFunction;
        private static LibVlcFunction<EditMediaParas> _editMediaParasFunction;
        private static LibVlcFunction<PlayNamedBroadcast> _playNamedBroadcastFunction;
        private static LibVlcFunction<StopNamedBroadcast> _stopNamedBroadcastFunction;
        private static LibVlcFunction<PauseNamedBroadcast> _pauseNamedBroadcastFunction;
        private static LibVlcFunction<SeekInNamedBroadcast> _seekInNamedBroadcastFunction;
        private static LibVlcFunction<ReturnJsonMessage> _returnJsonMessageFunction;
        private static LibVlcFunction<GetMediaPosition> _getMediaPositionFunction;
        private static LibVlcFunction<GetMediaTime> _getMediaTimeFunction;
        private static LibVlcFunction<GetMediaLength> _getMediaLengthFunction;
        private static LibVlcFunction<GetMediaBackRate> _getMediaBackRateFunction;
        private static LibVlcFunction<GetMediaEventManager> _getMediaEventManagerFunction;

        private readonly LibVlcEventCallBack _onVlmMediaAdded;
        private readonly LibVlcEventCallBack _onVlmMediaChanged;
        private readonly LibVlcEventCallBack _onVlmMediaInstanceStarted;
        private readonly LibVlcEventCallBack _onVlmMediaInstanceStatusEnd;
        private readonly LibVlcEventCallBack _onVlmMediaInstanceStatusError;
        private readonly LibVlcEventCallBack _onVlmMediaInstanceStatusInit;
        private readonly LibVlcEventCallBack _onVlmMediaInstanceStatusOpening;
        private readonly LibVlcEventCallBack _onVlmMediaInstanceStatusPause;
        private readonly LibVlcEventCallBack _onVlmMediaInstanceStatusPlaying;
        private readonly LibVlcEventCallBack _onVlmMediaInstanceStopped;
        private readonly LibVlcEventCallBack _onVlmMediaRemoved;

        private GCHandle _onVlmMediaAddedHandle;
        private GCHandle _onVlmMediaChangedHandle;
        private GCHandle _onVlmMediaInstanceStartedHandle;
        private GCHandle _onVlmMediaInstanceStatusEndHandle;
        private GCHandle _onVlmMediaInstanceStatusErrorHandle;
        private GCHandle _onVlmMediaInstanceStatusInitHandle;
        private GCHandle _onVlmMediaInstanceStatusOpeningHandle;
        private GCHandle _onVlmMediaInstanceStatusPauseHandle;
        private GCHandle _onVlmMediaInstanceStatusPlayingHandle;
        private GCHandle _onVlmMediaInstanceStoppedHandle;
        private GCHandle _onVlmMediaRemovedHandle;

        public VlcEventManager EventManager { get; private set; }

        public void VlmRelease()
        {
            _releaseVlmInstanceFunction.Delegate(InstancePointer);
        }

        public bool VlmAddBroadcast(String name, String input, String output, int options, String[] additionalOptions,
            int enable, int loop)
        {
            var nameHandle = InteropHelper.StringToPtr(name);
            var inputHandle = InteropHelper.StringToPtr(input);
            var outputHandle = InteropHelper.StringToPtr(output);

            var result = _newBroadCastInputFunction.Delegate(InstancePointer, nameHandle.AddrOfPinnedObject(),
                inputHandle.AddrOfPinnedObject(), outputHandle.AddrOfPinnedObject(), options,
                InteropHelper.StringArrayToPtr(additionalOptions), enable, loop) == 0;

            nameHandle.Free();
            inputHandle.Free();
            outputHandle.Free();

            return result;
        }

        public bool VlmAddVod(String name, String input, int options, String[] additionalOptions, int enable,
            String muxer)
        {
            var nameHandle = InteropHelper.StringToPtr(name);
            var inputHandle = InteropHelper.StringToPtr(input);
            var muxerHandle = InteropHelper.StringToPtr(muxer);

            var result = _newVodInputFunction.Delegate(InstancePointer, nameHandle.AddrOfPinnedObject(),
                inputHandle.AddrOfPinnedObject(), options, InteropHelper.StringArrayToPtr(additionalOptions), enable,
                muxerHandle.AddrOfPinnedObject()) == 0;

            nameHandle.Free();
            inputHandle.Free();
            muxerHandle.Free();

            return result;
        }

        public bool VlmDeleteMedia(String name)
        {
            var nameHandle = InteropHelper.StringToPtr(name);

            var result = _delBoroadcastOrOvdFunction.Delegate(InstancePointer, nameHandle.AddrOfPinnedObject()) == 0;

            nameHandle.Free();

            return result;
        }

        public bool VlmSetEnable(String name, int enable)
        {
            var nameHandle = InteropHelper.StringToPtr(name);

            var result = _mediaSwitchFunction.Delegate(InstancePointer, nameHandle.AddrOfPinnedObject(), enable) == 0;

            nameHandle.Free();

            return result;
        }

        public bool VlmSetOutput(String name, String output)
        {
            var nameHandle = InteropHelper.StringToPtr(name);
            var outputHandle = InteropHelper.StringToPtr(output);

            var result =
                _setMediaOutputFunction.Delegate(InstancePointer, nameHandle.AddrOfPinnedObject(),
                    outputHandle.AddrOfPinnedObject()) == 0;

            nameHandle.Free();
            outputHandle.Free();

            return result;
        }

        public bool VlmSetInput(String name, String input)
        {
            var nameHandle = InteropHelper.StringToPtr(name);
            var inputHandle = InteropHelper.StringToPtr(input);

            var result =
                _setMediaInputFunction.Delegate(InstancePointer, nameHandle.AddrOfPinnedObject(),
                    inputHandle.AddrOfPinnedObject()) == 0;

            nameHandle.Free();
            inputHandle.Free();

            return result;
        }

        public bool VlmAddInput(String name, String input)
        {
            var nameHandle = InteropHelper.StringToPtr(name);
            var inputHandle = InteropHelper.StringToPtr(input);

            var result =
                _addMediaInputFunction.Delegate(InstancePointer, nameHandle.AddrOfPinnedObject(),
                    inputHandle.AddrOfPinnedObject()) == 0;

            nameHandle.Free();
            inputHandle.Free();

            return result;
        }

        public bool VlmSetLoop(String name, int loop)
        {
            var nameHandle = InteropHelper.StringToPtr(name);

            var result = _setMediaLoopFunction.Delegate(InstancePointer, nameHandle.AddrOfPinnedObject(), loop) == 0;

            nameHandle.Free();

            return result;
        }

        public bool VlmSetMuxer(String name, String muxer)
        {
            var nameHandle = InteropHelper.StringToPtr(name);
            var muxerHandle = InteropHelper.StringToPtr(muxer);

            var result =
                _setVodMuxerFunction.Delegate(InstancePointer, nameHandle.AddrOfPinnedObject(),
                    muxerHandle.AddrOfPinnedObject()) == 0;

            nameHandle.Free();
            muxerHandle.Free();

            return result;
        }

        public bool VlmChangeMedia(String name, String input, String output, int options, String[] additionalOptions,
            int enable, int loop)
        {
            var nameHandle = InteropHelper.StringToPtr(name);
            var inputHandle = InteropHelper.StringToPtr(input);
            var outputHandle = InteropHelper.StringToPtr(output);

            var result = _editMediaParasFunction.Delegate(InstancePointer, nameHandle.AddrOfPinnedObject(),
                inputHandle.AddrOfPinnedObject(), outputHandle.AddrOfPinnedObject(), options,
                InteropHelper.StringArrayToPtr(additionalOptions), enable, loop) == 0;

            nameHandle.Free();
            inputHandle.Free();
            outputHandle.Free();

            return result;
        }

        public bool VlmPlay(String name)
        {
            var nameHandle = InteropHelper.StringToPtr(name);

            var result = _playNamedBroadcastFunction.Delegate(InstancePointer, nameHandle.AddrOfPinnedObject()) == 0;

            nameHandle.Free();

            return result;
        }

        public bool VlmStop(String name)
        {
            var nameHandle = InteropHelper.StringToPtr(name);

            var result = _stopNamedBroadcastFunction.Delegate(InstancePointer, nameHandle.AddrOfPinnedObject()) == 0;

            nameHandle.Free();

            return result;
        }

        public bool VlmPause(String name)
        {
            var nameHandle = InteropHelper.StringToPtr(name);

            var result = _pauseNamedBroadcastFunction.Delegate(InstancePointer, nameHandle.AddrOfPinnedObject()) == 0;

            nameHandle.Free();

            return result;
        }

        public bool VlmSeek(String name, float percentage)
        {
            var nameHandle = InteropHelper.StringToPtr(name);

            var result =
                _seekInNamedBroadcastFunction.Delegate(InstancePointer, nameHandle.AddrOfPinnedObject(), percentage) ==
                0;

            nameHandle.Free();

            return result;
        }

        public String VlmGetInfomation(String name)
        {
            var nameHandle = InteropHelper.StringToPtr(name);

            var result = _returnJsonMessageFunction.Delegate(InstancePointer, nameHandle.AddrOfPinnedObject());

            nameHandle.Free();

            return InteropHelper.PtrToString(result);
        }

        public float VlmGetPosition(String name, int id)
        {
            var nameHandle = InteropHelper.StringToPtr(name);

            var result = _getMediaPositionFunction.Delegate(InstancePointer, nameHandle.AddrOfPinnedObject(), id);

            nameHandle.Free();

            return result;
        }

        public int VlmGetTime(String name, int id)
        {
            var nameHandle = InteropHelper.StringToPtr(name);

            var result = _getMediaTimeFunction.Delegate(InstancePointer, nameHandle.AddrOfPinnedObject(), id);

            nameHandle.Free();

            return result;
        }

        public int VlmGetLength(String name, int id)
        {
            var nameHandle = InteropHelper.StringToPtr(name);

            var result = _getMediaLengthFunction.Delegate(InstancePointer, nameHandle.AddrOfPinnedObject(), id);

            nameHandle.Free();

            return result;
        }

        public int VlmGetRate(String name, int id)
        {
            var nameHandle = InteropHelper.StringToPtr(name);

            var result = _getMediaBackRateFunction.Delegate(InstancePointer, nameHandle.AddrOfPinnedObject(), id);

            nameHandle.Free();

            return result;
        }

        private void OnVlmMediaAdded(ref LibVlcEventArgs arg, IntPtr userData)
        {
            if (VlmMediaAdded != null)
            {
                VlmMediaAdded(this, new ObjectEventArgs<VlmMediaEventArgs>(arg.VlmMediaEvent));
            }
        }

        public event EventHandler<ObjectEventArgs<VlmMediaEventArgs>> VlmMediaAdded;

        private void OnVlmMediaRemoved(ref LibVlcEventArgs arg, IntPtr userData)
        {
            if (VlmMediaRemoved != null)
            {
                VlmMediaRemoved(this, new ObjectEventArgs<VlmMediaEventArgs>(arg.VlmMediaEvent));
            }
        }

        public event EventHandler<ObjectEventArgs<VlmMediaEventArgs>> VlmMediaRemoved;

        private void OnVlmMediaChanged(ref LibVlcEventArgs arg, IntPtr userData)
        {
            if (VlmMediaChanged != null)
            {
                VlmMediaChanged(this, new ObjectEventArgs<VlmMediaEventArgs>(arg.VlmMediaEvent));
            }
        }

        public event EventHandler<ObjectEventArgs<VlmMediaEventArgs>> VlmMediaChanged;

        private void OnVlmMediaInstanceStarted(ref LibVlcEventArgs arg, IntPtr userData)
        {
            if (VlmMediaInstanceStarted != null)
            {
                VlmMediaInstanceStarted(this, new ObjectEventArgs<VlmMediaEventArgs>(arg.VlmMediaEvent));
            }
        }

        public event EventHandler<ObjectEventArgs<VlmMediaEventArgs>> VlmMediaInstanceStarted;

        private void OnVlmMediaInstanceStopped(ref LibVlcEventArgs arg, IntPtr userData)
        {
            if (VlmMediaInstanceStopped != null)
            {
                VlmMediaInstanceStopped(this, new ObjectEventArgs<VlmMediaEventArgs>(arg.VlmMediaEvent));
            }
        }

        public event EventHandler<ObjectEventArgs<VlmMediaEventArgs>> VlmMediaInstanceStopped;

        private void OnVlmMediaInstanceStatusInit(ref LibVlcEventArgs arg, IntPtr userData)
        {
            if (VlmMediaInstanceStatusInit != null)
            {
                VlmMediaInstanceStatusInit(this, new ObjectEventArgs<VlmMediaEventArgs>(arg.VlmMediaEvent));
            }
        }

        public event EventHandler<ObjectEventArgs<VlmMediaEventArgs>> VlmMediaInstanceStatusInit;

        private void OnVlmMediaInstanceStatusOpening(ref LibVlcEventArgs arg, IntPtr userData)
        {
            if (VlmMediaInstanceStatusOpening != null)
            {
                VlmMediaInstanceStatusOpening(this, new ObjectEventArgs<VlmMediaEventArgs>(arg.VlmMediaEvent));
            }
        }

        public event EventHandler<ObjectEventArgs<VlmMediaEventArgs>> VlmMediaInstanceStatusOpening;

        private void OnVlmMediaInstanceStatusPlaying(ref LibVlcEventArgs arg, IntPtr userData)
        {
            if (VlmMediaInstanceStatusPlaying != null)
            {
                VlmMediaInstanceStatusPlaying(this, new ObjectEventArgs<VlmMediaEventArgs>(arg.VlmMediaEvent));
            }
        }

        public event EventHandler<ObjectEventArgs<VlmMediaEventArgs>> VlmMediaInstanceStatusPlaying;

        private void OnVlmMediaInstanceStatusPause(ref LibVlcEventArgs arg, IntPtr userData)
        {
            if (VlmMediaInstanceStatusPause != null)
            {
                VlmMediaInstanceStatusPause(this, new ObjectEventArgs<VlmMediaEventArgs>(arg.VlmMediaEvent));
            }
        }

        public event EventHandler<ObjectEventArgs<VlmMediaEventArgs>> VlmMediaInstanceStatusPause;

        private void OnVlmMediaInstanceStatusEnd(ref LibVlcEventArgs arg, IntPtr userData)
        {
            if (VlmMediaInstanceStatusEnd != null)
            {
                VlmMediaInstanceStatusEnd(this, new ObjectEventArgs<VlmMediaEventArgs>(arg.VlmMediaEvent));
            }
        }

        public event EventHandler<ObjectEventArgs<VlmMediaEventArgs>> VlmMediaInstanceStatusEnd;

        private void OnVlmMediaInstanceStatusError(ref LibVlcEventArgs arg, IntPtr userData)
        {
            if (VlmMediaInstanceStatusError != null)
            {
                VlmMediaInstanceStatusError(this, new ObjectEventArgs<VlmMediaEventArgs>(arg.VlmMediaEvent));
            }
        }

        public event EventHandler<ObjectEventArgs<VlmMediaEventArgs>> VlmMediaInstanceStatusError;
    }
}