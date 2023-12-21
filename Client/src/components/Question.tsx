interface Props {
    onAnswerChange: (index:number) => void
    item: QuestionItem
    userAnswer: number
}

export default function Question({item, onAnswerChange, userAnswer}:Props) {
    return(
        <div>
            <h5 className="text-center">{item.question}</h5>
            { item.answers.map((answer, index) => (
                <div key={index} onClick={() => onAnswerChange(index)}
                className={(index === userAnswer ? "selected-answer-style" : "answer-style") + " mb-2"}
                >{answer}</div>
            ))}
        </div>
    )
}