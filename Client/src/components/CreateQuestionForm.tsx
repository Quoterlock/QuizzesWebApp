import { ChangeEvent, useState } from "react"

interface Props {
    onChange:(question:QuestionItem) => void
    question:QuestionItem
}

export default function CreateQuestionForm({onChange, question}:Props) {
    console.log(question)

    const addOption = () => {
        onChange({...question, options:[...question.options, GetDefaultOption()]})
    }

    const onQuestionLabelChange = (e:ChangeEvent<HTMLInputElement>) => {
        onChange({...question, text:e.currentTarget.value})
    }

    const onOptionChange = (index:number, e:ChangeEvent<HTMLInputElement>) => {
        const newOptions = [...question.options];
        newOptions[index] = {...newOptions[index], text:e.currentTarget.value}
        onChange({...question, options:newOptions})
    }

    const onCorrectAnswerChange = (index:number) => {
        if(question.correctAnswerIndex !== index)
            onChange({...question, correctAnswerIndex:index})
    }
    
    const removeOption = (index:number) => {
        var newQuiestion = question
        newQuiestion.options = question.options.filter((elem,curIndex)=>curIndex !== index)
        onChange(newQuiestion)
    }

    return(<div className="block-style mb-3">
        <div className="d-grid form-inputs mb-3">
            <label>Question</label>
            <input type="text" onChange={onQuestionLabelChange}/>
        </div>
        <div >
            <label>Options</label>
            { question.options.map((option,index)=> <div key={index} className="edit-option-style form-inputs mb-2">
            <div className={(question.correctAnswerIndex === index?"correct":"default")+"-answer-select me-2"} onClick={()=>onCorrectAnswerChange(index)}></div>
            <input type="text" className="option-textbox" value={option.text} 
                onChange={(e:ChangeEvent<HTMLInputElement>) => onOptionChange(index,e)}/>
            <button className="btn active-btn ms-2" onClick={() => removeOption(index)}>-</button>
        </div>)}
        </div>
        <div className="d-grid">
            <button className="btn active-btn" onClick={addOption}>Add option</button>
        </div>
    </div>)
}

function GetDefaultOption(): OptionItem {
    return({text:""})
}
function GetDefaultQuestion(): QuestionItem {
    return({text:"",options:[{text:""}],correctAnswerIndex:0})
}