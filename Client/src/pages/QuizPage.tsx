import Question from "../components/Question"
import { QuizLayout } from "../layouts/QuizLayout"
import { useState } from "react"

export default function QuizPage(){
    const questions = InitQuestions()
    const [selectedQuestion, setSelectedQuestion] = useState(0)
    const [userAnswers, setAnswer] = useState<number[]>(new Array(questions.length))
    const [showAnswer, setShowAnswer] = useState(false)


    const hNextQuestion = () => {
        setSelectedQuestion(selectedQuestion+1)
        userAnswers
    }
    const hPrevQuestion = () => {
        setSelectedQuestion(selectedQuestion-1)
    }

    const hOnAnswerChange = (index:number) => {
        let answers = userAnswers
        answers[selectedQuestion] = index 
        setAnswer(answers)
        console.log("Change question " + selectedQuestion + " answer " + index)
        setShowAnswer(false)
        setShowAnswer(true)
    }

    const hShowAnswerChange = () => {
        setShowAnswer(!showAnswer)
    }

    const redText = {
        color: 'red'
    }
    const greenText = {
        color: 'green'
    }

    return(
    <QuizLayout>
        <div className="background-color col-2 mx-auto">
            <h1>Question</h1>
            <Question onAnswerChange={hOnAnswerChange} item={questions[selectedQuestion]}/>
            {showAnswer && <div>
                {questions[selectedQuestion].correctAnswerIndex === userAnswers[selectedQuestion] ?
                <p style={greenText}>Correct!</p> : <p style={redText}>Wrong!</p> }
                </div>}
            <button className="btn active-btn" onClick={hPrevQuestion}>Prev</button>
            <button className="btn btn-primary" onClick={hNextQuestion}>Next</button>
            <button className="btn btn-primary" onClick={hShowAnswerChange}>Finish</button>
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
