import { useState } from "react"
import Question from "./Question"

interface Props {
    quiz:QuizItem
    onDone: (answers: number[]) => void
}

export default function Quiz({quiz, onDone}:Props) {
    const [selectedQuestion, setSelectedQuestion] = useState(0)
    const [userAnswers, setAnswer] = useState<number[]>(new Array(quiz.questions.length))
    
    const hNextQuestion = () => {
        if(selectedQuestion < quiz.questions.length-1)
            setSelectedQuestion(selectedQuestion+1)
    }
    
    const hPrevQuestion = () => {
        if(selectedQuestion > 0)
            setSelectedQuestion(selectedQuestion-1)
    }

    const hOnAnswerChange = (index:number) => {
        setAnswer(
            (prev) => {
                const updated = [...prev];
                updated[selectedQuestion] = index;
                return updated
            })
    }

    return(
        <div>
            <h1>{"Quiz: " + quiz.title}</h1>
            <div className="block-style mb-3">
                <p className="text-center">{
                    (selectedQuestion + 1).toString() + "/" + quiz.questions.length.toString()
                }</p>
                <Question 
                    onAnswerChange={hOnAnswerChange} 
                    question={quiz.questions[selectedQuestion]} 
                    userAnswer={userAnswers[selectedQuestion]}/>
            </div>

            <div className="container-flex">
                <div className="row g-2 mb-3">
                    <div className="d-grid col">
                        <button className="btn active-btn" onClick={hPrevQuestion}>Prev</button>
                    </div>
                    <div className="d-grid col">
                        <button className="btn active-btn" onClick={hNextQuestion}>Next</button>
                    </div>
                </div>
                <div className="row g-2">
                    <div className="d-grid col">
                    <button className="btn active-btn" onClick={()=> onDone(userAnswers)}>Finish</button>
                    </div>
                </div>
            </div>
        </div>
    )
}