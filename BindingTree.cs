using System;
using System.Collections.Generic;
using System.Text;

namespace whkd
{
    class BindingTree
    {
        ICollection<BindingTree> children = new List<BindingTree>();
        public KeyChord chord;
        public string command;

        // TODO: do this properly
        public void MergeChildren(ICollection<BindingTree> otherChildren)
        {
            bool found = false;
            foreach (var otherChild in otherChildren)
            {
                found = false;
                foreach (var child in children)
                {
                    if (child.chord == otherChild.chord)
                    {
                        child.MergeChildren(otherChild.children);
                        // Add command if new
                        if (child.command is null && !(otherChild.command is null))
                        {
                            child.command = otherChild.command;
                        }
                        // Both have non-null commands
                        else if (!(otherChild.command is null))
                        {
                            throw MergeError.Create(child.command, otherChild.command);
                        }
                        found = true;
                    }
                }
                if (!found)
                {
                    children.Add(otherChild);
                }
            }
        }

        public BindingTree TryMatchChordInChildren(KeyChord chord)
        {
            foreach (var child in children)
            {
                if (child.chord == chord)
                {
                    return child; // up to caller to check command string
                }
            }
            return null; // not found
        }
    }
}
