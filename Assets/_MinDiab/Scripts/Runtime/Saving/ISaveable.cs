namespace WizardsCode.MinDiab.Core
{
    public interface ISaveable
    {
        object CaptureState();
        void RestoreState(object state);
    }
}