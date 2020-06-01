namespace MM26TestServer.Models
{
    public class Configuration
    {
        /// <summary>
        /// State data, has to use int[] here, as <c>System.Text.Json</c>
        /// considers do not convert <c>[1, 2, 3]</c> to byte[]
        /// </summary>
        /// <value></value>
        public int[] State { get; set; }

        /// <summary>
        /// Changes
        /// </summary>
        /// <value></value>
        public Change[] Changes { get; set; }
    }
}
