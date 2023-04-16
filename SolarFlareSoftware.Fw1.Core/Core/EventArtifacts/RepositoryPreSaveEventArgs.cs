namespace SolarFlareSoftware.Fw1.Core.Events
{
    public class RepositoryPreSaveEventArgs : EventArgs
    {
        public bool CancelSave { get; set; } = false;
        public RepositoryPreSaveEventArgs()
        {

        }
    }
}
