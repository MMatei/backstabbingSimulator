using UnityEngine;

/*
 * Model that represents any land-owning entity that can, however loosely, be qualified as a state
 */
public class State {

    internal Texture2D banner;
    internal string name;
    internal Character leader;

    public State(string name, Texture2D banner)
    {
        this.name = name;
        this.banner = banner;
    }
}
