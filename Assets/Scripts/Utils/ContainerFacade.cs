using WarlockBrawls.Utils;

namespace Utils
{
    public static class ContainerFacade
    {
        public static HeroDataContainer HeroesData => HeroDataContainer.Instance;
        public static SpellSettingsContainer SpellSettings => SpellSettingsContainer.Instance;
    }
}