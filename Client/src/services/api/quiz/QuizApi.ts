import { useEffect, useState } from "react"

var apiPath:string

export class QuizApi implements IQuizApi{

    constructor(apiRoute:string) {
        apiPath = `${apiRoute}/quizzes`
    }

    async SaveResultAsync(result: QuizResult): Promise<RequesResult> {
        const token = localStorage.getItem("jwt-token")
        const response = await fetch(`${apiPath}/save-result`, {
            method:"POST",
            headers: {
                "Content-Type":"application/json",
                "Authorization": `Bearer ${token}`
            },
            
            body: JSON.stringify(result)
        });
        if(response.ok)  {
            return { code:200, message:"all is ok"}
        } else {
            return {code:response.status, message:"server error"}
        }
    }


    async CreateNewQuiz(quiz: QuizItem): Promise<RequesResult> {
        const token = localStorage.getItem("jwt-token")
        const result = await fetch(`${apiPath}/create`, {
            method:"POST",
            headers: {
                "Content-Type":"application/json",
                "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify(quiz)
        });
        if(result.ok)  {
            return { code:200, message:"all is ok"}
        } else {
            return {code:result.status, message:"server error"}
        }
    }

    GetList(startIndex: number, endIndex: number): QuizListItem[] {
        const [items, setItems] = useState<QuizListItem[]>([])    
        useEffect(()=>{
            const fetchData = async () => {
                try{
                    console.log(`${apiPath}/list?startIndex=${startIndex}&endIndex=${endIndex}`)
                    const responce = await fetch(`${apiPath}/list?startIndex=${startIndex}&endIndex=${endIndex}`)
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
    
    async GetByIdAsync(id: string): Promise<QuizAndResults> {
        try{
            const responce = await fetch(`${apiPath}/${id}`)
            if(!responce.ok) {
                throw new Error ("failed to fetch data");
            }
            const data: QuizAndResults = await responce.json()
            return data
        } catch (error) {
            console.error("Error fetching data:",error);
            throw error;
        }
    }
}