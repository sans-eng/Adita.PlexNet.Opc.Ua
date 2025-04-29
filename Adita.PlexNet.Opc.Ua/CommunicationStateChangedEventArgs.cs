namespace Adita.PlexNet.Opc.Ua
{
    /// <summary>  
    /// Provides data for the communication state changed event.
    /// </summary>  
    public class CommunicationStateChangedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommunicationStateChangedEventArgs"/> class.
        /// </summary>
        /// <param name="state">The current communication state.</param>  
        public CommunicationStateChangedEventArgs(CommunicationState state)
        {
            State = state;
        }

        /// <summary>
        /// Gets the current communication state.
        /// </summary>
        public CommunicationState State { get; }
    }
}
