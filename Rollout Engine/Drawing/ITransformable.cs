namespace Rollout.Drawing
{
    public interface ITransformable
    {
        float X { get; set; }
        float Y { get; set; }
        float Scale { get; set; }
        float Rotation { get; set; }
    }
}