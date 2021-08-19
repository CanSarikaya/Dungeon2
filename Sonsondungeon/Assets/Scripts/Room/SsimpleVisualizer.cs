using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class SsimpleVisualizer : MonoBehaviour
    {
        public LlSystemGenerator Llsystem;
        List<Vector3> positions = new List<Vector3>();
        public GameObject prefab,room;
        public Material lineMaterial;
        private int length = 1;
        private float angle = 90;

        public int Length
        {
            get
            {
                if (length > 0)
                {
                    return length;
                }
                else
                {
                    return 1;
                }
            }
            set => length = value;
        }

        private void Start()
        {
            var sequence = Llsystem.GenerateSentence();
            VisualizeSequence(sequence);
        }

        private void VisualizeSequence(string sequence)
        {
               GameObject[] Roomstart;
               Roomstart = GameObject.FindGameObjectsWithTag("Roomstart");
               foreach(GameObject room in Roomstart)
               {
            Stack<AagentParameters> savePoints = new Stack<AagentParameters>();
  
           
            var currentPosition = room.transform.position;

            Vector3 direction = Vector3.forward;
            Vector3 tempPosition = room.transform.position;

            positions.Add(currentPosition);

            foreach (var letter in sequence)
            {
                EncodingLetters encoding = (EncodingLetters)letter;
                switch (encoding)
                {
                    case EncodingLetters.save:
                        savePoints.Push(new AagentParameters
                        {
                            position = currentPosition,
                            direction = direction,
                            length = Length
                        });
                        break;
                    case EncodingLetters.load:
                        if (savePoints.Count > 0)
                        {
                            var agentParameter = savePoints.Pop();
                            currentPosition = agentParameter.position;
                            direction = agentParameter.direction;
                            Length = agentParameter.length;
                        }
                        else
                        {
                            throw new System.Exception("Dont have saved point in our stack");
                        }
                        break;
                    case EncodingLetters.draw:
                        tempPosition = currentPosition;
                        currentPosition += direction * length;
                        DrawLine(tempPosition, currentPosition, Color.red);
                        
                        positions.Add(currentPosition);
                        break;
                    case EncodingLetters.turnRight:
                        direction = Quaternion.AngleAxis(angle, Vector3.up) * direction;
                        break;
                    case EncodingLetters.turnLeft:
                        direction = Quaternion.AngleAxis(-angle, Vector3.up) * direction;
                        break;
                    default:
                        break;
                }
            }

           }
        }

        private void DrawLine(Vector3 start, Vector3 end, Color color)
        {
            GameObject line =  GameObject.Find("Wall");
            Instantiate(room, start, Quaternion.identity);
            line.transform.position = start;
            Debug.Log("Hello: " + start);
            Debug.Log("end: " + end);
             float scalex = Mathf.Abs(start.x - end.x);
             float scalez = Mathf.Abs(start.z - end.z);

             if(scalex==0)
             {
                  scalex++;
             }
             if (scalez==0 )
             {
                  scalez++;
             }
            line.transform.localScale = new Vector3(scalex,1,scalez);
            GameObject line1 = new GameObject("line1");
            line1.transform.position = start;
            line.transform.position = (start+end)/2;
            if(Mathf.Abs(start.x-end.x)<1 & start.z<end.z)
            {
            line.transform.position = new Vector3(line.transform.position.x,1,line.transform.position.z+(length/2));
            }
            if(Mathf.Abs(start.z-end.z)<1 & start.x<end.x)
            {
            line.transform.position = new Vector3(line.transform.position.x-(length/2),1,line.transform.position.z);
            }
            if(Mathf.Abs(start.x-end.x)<1 & start.z>end.z)
            {
            line.transform.position = new Vector3(line.transform.position.x,1,line.transform.position.z+(length/2));
            }
                        if(Mathf.Abs(start.z-end.z)<1 & start.x>end.x)
            {
            line.transform.position = new Vector3(line.transform.position.x-(length/2),1,line.transform.position.z);
            }
            var lineRenderer = line1.AddComponent<LineRenderer>();
            lineRenderer.material = lineMaterial;
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.SetPosition(0, end);
            lineRenderer.SetPosition(1, start);
            
        }

        public enum EncodingLetters
        {
            unknown = '1',
            save = '[',
            load = ']',
            draw = 'F',
            turnRight = '+',
            turnLeft = '-'
        }
    }
