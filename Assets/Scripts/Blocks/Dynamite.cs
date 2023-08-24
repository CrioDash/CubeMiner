using UnityEngine;

namespace Fruit
{
    public class Dynamite:MonoBehaviour, ISlice
    {
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if(other.CompareTag("Player"))
                Slice();
        }
        
        public void Slice()
        {
            
        }
    }
}