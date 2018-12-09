public class InnovationGenerator
{
    private static int innovation = 0;
    public static int GetInnovation()
    {
        innovation++;
        return innovation;
    }
}