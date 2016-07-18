using System;

namespace SmallDahuaLib
{
    /// <summary>
    /// This attribute helps to serialize the BinaryPackets.
    /// </summary>
    public class FieldAttribute : Attribute
    {
        /// <summary>
        /// Length of the field.
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// Order of the field.
        /// </summary>
        public int Order { get; private set; }

        /// <summary>
        /// Constructor a new FieldAttribute with specified params.
        /// </summary>
        public FieldAttribute(int order, int length = 0)
        {
            Order = order;
            Length = length;
        } 
    }
}
