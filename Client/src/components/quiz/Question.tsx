interface Props {
    question: QuestionItem
    onAnswerChange: (index:number) => void
    userAnswer: number
}

export default function Question({question, onAnswerChange, userAnswer}:Props) {
    return(
        <div>
            <h5 className="text-center">{question.text}</h5>
            { question.options.map((option, index) => (
                <div key={index} onClick={() => onAnswerChange(index)}
                className={(index === userAnswer ? "selected-answer-style" : "answer-style") + " mb-2"}
                >{option.text}</div>
            ))}
        </div>
    )
}