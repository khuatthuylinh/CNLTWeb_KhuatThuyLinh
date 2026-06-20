package com.example.studentmanagement.controller;

import com.example.studentmanagement.model.Student;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

@Controller
public class StudentController {

    @GetMapping("/student/info")
    public String info(Model model) {

        Student student =
                new Student(
                        "Nguyễn Văn A",
                        20,
                        "Công nghệ thông tin"
                );

        model.addAttribute("student", student);

        return "student/info";
    }
}