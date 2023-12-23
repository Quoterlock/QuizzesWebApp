import { useEffect, useState } from "react"

const apiPath = "https://localhost:7181/api"

export class QuizApi implements IQuizApi{
    GetList(startIndex: number, endIndex: number): QuizListItem[] {
        const [items, setItems] = useState<QuizListItem[]>([])    
        useEffect(()=>{
            const fetchData = async () => {
                try{
                    console.log(`${apiPath}/Quizzes/List?startIndex=${startIndex}&endIndex=${endIndex}`)
                    const responce = await fetch(`${apiPath}/Quizzes/List?startIndex=${startIndex}&endIndex=${endIndex}`)
                    const jsonResponce = await responce.json()
                    console.log(jsonResponce)
                    setItems(jsonResponce)
                }
                catch(error){
                    console.error("Error fetching data: ", error)
                }
            }
            fetchData()
        }, [])
        console.log(items)
        return items
    }
    GetById(id: string, setItem:(item:QuizItem)=>void){
        
        const hOnSet = (data:QuizItem|undefined) => {
            if(data != null) {
                console.log(data)
                setItem(data)
            }
            console.log("wait...")
        }
    
        useEffect(()=>{
            fetch(`${apiPath}/Quizzes/${id}`)
                .then(res => res.json())
                .then(data => {
                    hOnSet(data)
                })
        }, [id]) 
        
       return null
    }
    
    async GetByIdAsync(id: string): Promise<QuizItem> {
        try{
            const responce = await fetch(`${apiPath}/Quizzes/${id}`)
            if(!responce.ok) {
                throw new Error ("failed to fetch data");
            }
            const data: QuizItem = await responce.json()
            return data
        } catch (error) {
            console.error("Error fetching data:",error);
            throw error;
        }
    }
}