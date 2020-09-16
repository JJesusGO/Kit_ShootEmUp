using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;

namespace SEUP{

    [System.Serializable]
    public struct Data{
        
        [SerializeField]
        public string referencia;
        [SerializeField]
        public string valor;

        public Data(string referencia,string valor){
            this.referencia = referencia;
            this.valor = valor;
        }

    }
    [System.Serializable]
    public class DataGuardar{
        [SerializeField]
        private Data[] data = null;

        public void SetData(Data []data){
            this.data = data;
        }

        public Data GetData(int i){
            return data[i];
        }

        public int GetDataCount(){
            if (data == null)
                return 0;
            return data.Length;
        }

        public bool Crear(bool forzar = false) {            
            string fullpath = Application.dataPath + "/info.scythe";
            if (forzar)
                Borrar();
            if (!File.Exists(fullpath))
                Guardar();
            return true;
        }
        public bool Guardar() {
            string fullpath = Application.dataPath + "/info.scythe";
            try{                

                BinaryFormatter formato = new BinaryFormatter();

                FileStream stream = File.Open(fullpath,FileMode.Create,FileAccess.Write);
                formato.Serialize(stream, this);

                stream.Close();

            }
            catch(Exception){

            }
            return true;
        }
        public bool Cargar() {          
            string fullpath = Application.dataPath + "/info.scythe";

            if (!File.Exists(fullpath))
                return false;

            BinaryFormatter formato = new BinaryFormatter();
            FileStream stream = File.Open(fullpath, FileMode.Open, FileAccess.Read);

            DataGuardar data = (DataGuardar)formato.Deserialize(stream);         
            SetData(data.data);

            stream.Close();
           
            return true;
        } 
        public bool Borrar(){
            string fullpath = Application.dataPath + "/info.scythe";
            if (File.Exists(fullpath))
                File.Delete(fullpath);
            return true;        
        }


    }

    public class Guardado : MonoBehaviour{

        [SerializeField]
        private Data[] data = null;


        private static Guardado instancia = null;
        private DataGuardar archivo = new DataGuardar();

        private void Awake(){
            instancia = this;
        }
     
        public void Cargar(){

            archivo.SetData(data);

            archivo.Crear();
            archivo.Cargar();    

            for (int i = 0; i < archivo.GetDataCount(); i++)
                SetData(archivo.GetData(i));                      

        }
        public void Guardar(){
            archivo.SetData(data);
            archivo.Guardar();
        }
        public void Borrar(){
            archivo.Borrar();
        }

        public void SetData(Data data){
            for (int i = 0; i < this.data.Length; i++)
                if (this.data[i].referencia == data.referencia){
                    this.data[i].valor = data.valor;
                    break;
                }                   
        }
        public void SetData(string referencia,string valor){
            for (int i = 0; i < data.Length; i++)
                if (data[i].referencia == referencia){
                    data[i].valor = valor;
                    break;
                }
        }
            
        public Data GetData(string referencia){
            if (data == null)
                return new Data("","");
            for (int i = 0; i < data.Length; i++)
                if (data[i].referencia == referencia)
                    return data[i];
            return new Data("", "");
        }
        public Data GetData(int i){
            return data[i];
        }

        public int GetDataCount(){
            return data.Length;
        }

        public static Guardado GetInstancia(){
            if (instancia == null)
                instancia = GameObject.FindObjectOfType<Guardado>();
            return instancia;
        }
    
    }

}

