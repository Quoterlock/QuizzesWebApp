import { Link } from "react-router-dom"

interface Props {
    userAnswers: number[]
    questions: QuestionItem[]
    onRestart: () => void
}

export default function QuizResults({userAnswers, questions, onRestart}: Props) {
    return (<div className="container">
        <div className="row d-grid mb-3">
            <button className="btn active-btn" onClick={onRestart}>Restart</button>
        </div>
        <div className="row">{ 
        questions.map((question, index) => (
            <div key={index} className="block-style mb-3">
                <p className={ question.correctAnswerIndex === userAnswers[index] 
                ? "correct-answer-label-style" 
                : "wrong-answer-label-style"}>{"Your answer: " + ((userAnswers[index] === undefined)? "none": question.options[userAnswers[index]].text)}</p>
                <p>{"Correct answer: " + question.options[question.correctAnswerIndex].text}</p>
            </div>))}
        </div>
    </div>)
}
