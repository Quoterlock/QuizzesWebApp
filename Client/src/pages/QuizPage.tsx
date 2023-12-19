import Question from "../components/Question"
import { QuizLayout } from "../layouts/QuizLayout"
import { useState } from "react"

export default function QuizPage(){
    const questions = InitQuestions()
    const [selectedQuestion, setSelectedQuestion] = useState(0)
    const [userAnswers, setAnswer] = useState<number[]>(new Array(questions.length))

    const hNextQuestion = () => {
        setSelectedQuestion(selectedQuestion+1)
        userAnswers
    }
    const hPrevQuestion = () => {
        setSelectedQuestion(selectedQuestion-1)
    }

    return(
    <QuizLayout>
        <div className="background-color">
            <h1>Question</h1>
            <Question item={questions[selectedQuestion]}/>
            <button className="btn active-btn" onClick={hPrevQuestion}>Prev</button>
            <button className="btn btn-primary" onClick={hNextQuestion}>Next</button>
            <button className="btn btn-primary" onClick={hNextQuestion}>Finish</button>
        </div>
    </QuizLayout>)
}

function InitQuestions(): QuizItem[] {
    const arr: QuizItem[] = [
        {
            id: "1",
            question: "1",
            answers: [
                "1","2","3"
            ],
            correctAnswerIndex:2
        },
        {
            id: "2",
            question: "2",
            answers: [
                "1","2","3"
            ],
            correctAnswerIndex:2
        }
    ]
    return (arr)
}
