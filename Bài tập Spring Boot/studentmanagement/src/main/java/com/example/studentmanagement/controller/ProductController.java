package com.example.studentmanagement.controller;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

@Controller
@RequestMapping("/product")
public class ProductController {

    @GetMapping("/detail/{id}")
    @ResponseBody
    public String detail(@PathVariable Integer id) {
        return "Product ID = " + id;
    }

    @GetMapping("/category")
    @ResponseBody
    public String category(@RequestParam(required = false) String name) {

        if (name == null || name.trim().isEmpty()) {
            return "Lỗi: Chưa nhập tên danh mục";
        }

        return "Category = " + name;
    }
}