import { useEffect, useState } from "react"

const apiPath = "local..."


export function GetListOfQuiz(startIndex:number, endIndex:number) : QuizListItem[] {
    
    const [items, setItems] = useState<QuizListItem[]>([])    
    useEffect(()=>{
        const fetchData = async () => {
            try{
                const responce = await fetch('${apiPath}/quiz/list?startIndex=${startIndex}&endIndex=${endIndex}')
                const jsonResponce = await responce.json()
                setItems(jsonResponce)
            }
            catch(error){
                console.error("Error fetching data: ", error)
            }
        }
        fetchData()
    }, [])
    return items
}

export function GetQuizById(id:string) : QuizItem {
    const [item, setItem] = useState<QuizItem>(getDefaultQuiz())    
    useEffect(()=>{
        const fetchData = async () => {
            try{
                const responce = await fetch('${apiPath}/quiz/byid?quizId=${id}')
                const jsonResponce = await responce.json()
                setItem(jsonResponce)
            }
            catch(error){
                console.error("Error fetching data: ", error)
            }
        }
        fetchData()
    }, [])
    return item
}

function getDefaultQuiz() : QuizItem {
    return ({
        questions: [],
        author: "",
        title: "",
        rate: 0
    })
}

export default [GetListOfQuiz, GetQuizById]