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
        console.log(userAnswers)
    }
    const hPrevQuestion = () => {
        setSelectedQuestion(selectedQuestion-1)
        console.log(userAnswers)
    }

    const hOnAnswerChange = (index:number) => {
        let answers = userAnswers
        answers[selectedQuestion] = index 
        setAnswer(answers)
        console.log("Change question " + selectedQuestion + " answer " + index)
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
        <div className="col-6 mx-auto">
            <div className="block-style mb-3">
                <h1>{
                    "Question " + (selectedQuestion + 1).toString() + "/" + questions.length.toString()
                }</h1>
                <Question onAnswerChange={hOnAnswerChange} 
                    item={questions[selectedQuestion]} 
                    userAnswer={userAnswers[selectedQuestion]}/>
                {showAnswer && <div>
                    {questions[selectedQuestion].correctAnswerIndex === userAnswers[selectedQuestion] ?
                    <p className="correct-answer-label-style text-center">Correct!</p> : <p className="wrong-answer-label-style text-center">Wrong!</p> }
                    </div>}    
            </div>
            <button className="btn active-btn" onClick={hPrevQuestion}>Prev</button>
            <button className="btn active-btn" onClick={hNextQuestion}>Next</button>
            <button className="btn active-btn" onClick={hShowAnswerChange}>Finish</button>
        </div>
    </QuizLayout>)
}

function InitQuestions(): QuizItem[] {
    const arr: QuizItem[] = [
        {
            id: "1",
            question: "Як перекладається слово Wizard?",
            answers: [
                "а) Чарівник","б) Метелиця","в) Привид"
            ],
            correctAnswerIndex:0
        },
        {
            id: "2",
            question: "Як перекладається слово Adventure?",
            answers: [
                "а) Робота","б) Розвиток", "в) Пригода"
            ],
            correctAnswerIndex:2
        }
    ]
    return (arr)
}
